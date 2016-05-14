using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Skill
{
    Labourer,
    Carpenter,
    Blacksmith,
    Teacher,
    Miner,
    Lumberjack,
    Rifleman
}


public enum State
{
    Idle,
    action,
    move
}

public delegate void actfunc();


public enum action
{
    Family,
    Educate,
    Train,
    CutTree,
    Mine,
    Store,
    Move,
    Smelt,
    Quarry,
    SawWood,
    MakeTool,
    Combat
}

public class Person : MonoBehaviour {

    
    public List<Skill> Skills = new List<Skill>();
    [HideInInspector]
    public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();

    public List<action> ToDoList = new List<action>();
    
    public IVec2 currentMapPos = new IVec2();

    public State currentstate = State.Idle;

    Map CurrentMap = null;
    PathFinder finder = null;

    uint PathID = 0;
    public int BusyTime = 0;
  
    float time = 0;

	// Use this for initialization
	IEnumerator Start () {
        finder = GameObject.FindGameObjectWithTag("Map").GetComponent<PathFinder>();

        yield return new WaitForSeconds(1f);

        if (Map.CurrentMap)
            currentMapPos = Map.CurrentMap.getTileFromPos(transform.position);

        ResetResources();
        Skills.Add(Skill.Labourer);
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if(time > Map.TimeUnit)
        {
            time = 0;
            if(BusyTime >0)
            {
                BusyTime--;
                return;
            }
            if(ToDoList.Count >0)
            {
                switch (ToDoList[0])
                {
                    case action.Family:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);
                            //check for another person 
                            if (CurrentBuilding.GetNonBusyPersonInBuilding() != null)
                            {
                                SetBusy(20);
                                CurrentBuilding.GetNonBusyPersonInBuilding().SetBusy(20);
                                if (CurrentBuilding.m_buildingtype == BuildingType.turfHut)
                                {
                                    Map.CurrentMap.AddPerson(currentMapPos);
                                }
                                else if( CurrentBuilding.m_buildingtype == BuildingType.House)
                                {
                                    Map.CurrentMap.AddPerson(currentMapPos);
                                    Map.CurrentMap.AddPerson(currentMapPos);
                                }
                            }
                        }

                        break;
                    case action.Educate:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);

                            if (Skills.Contains(Skill.Rifleman)
                                && CurrentBuilding.m_buildingtype == BuildingType.Barracks
                                && CurrentBuilding.GetNonBusyPersonInBuilding() != null)
                            {
                                var other = CurrentBuilding.GetNonBusyPersonInBuilding();
                                SetBusy(30);
                                other.SetBusy(30);
                                other.Skills.Add(Skill.Rifleman);
                            }
                        }
                        else
                        {
                            Person other = Map.CurrentMap.GetNonBusyPersonAt(currentMapPos);
                            if (other == null)
                                break;

                            foreach (var item in Skills)
                            {
                                if(!other.Skills.Contains(item))
                                {
                                    other.SetBusy(100);
                                    SetBusy(100);
                                    other.Skills.Add(item);
                                    break;
                                }
                            }
                        }
                        break;
                    case action.Train:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);

                            if (CurrentBuilding.m_buildingtype == BuildingType.School)
                            {
                                var other = CurrentBuilding.GetNonBusyPersonInBuilding();
                                while (other != null)
                                {
                                    foreach (var item in Skills)
                                    {
                                        if (!other.Skills.Contains(item))
                                        {
                                            other.SetBusy(50);
                                            SetBusy(50);
                                            other.Skills.Add(item);
                                            break;
                                        }
                                    }
                                    other = CurrentBuilding.GetNonBusyPersonInBuilding();
                                }
                            }
                            
                        }
                        break;
                    case action.CutTree:
                        if (Map.CurrentMap.getObject(currentMapPos) is ResourceTile)
                        {
                            ResourceTile tile = (ResourceTile)Map.CurrentMap.getObject(currentMapPos);
                            if(tile.m_resource == ResourceType.Timber
                                && Skills.Contains(Skill.Lumberjack))
                            {
                                Resources[ResourceType.Timber] += tile.GatherResource(ResourceType.Timber);
                                SetBusy(5);
                            }
                        }
                        break;
                    case action.Mine:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);

                            if (Skills.Contains(Skill.Miner)
                                && CurrentBuilding.m_buildingtype == BuildingType.Mine)
                            {
                                SetBusy(5);   
                                //random chance to get coal or ore
                                ResourceType r = ((Mathf.FloorToInt(Random.value) % 2) == 0) ? ResourceType.Coal : ResourceType.Ore;
                                Resources[r]++;
                            }
                        }
                        else if(Map.CurrentMap.getObject(currentMapPos) is ResourceTile)
                        {
                            ResourceTile tile = (ResourceTile)Map.CurrentMap.getObject(currentMapPos);
                            if (tile.m_resource == ResourceType.Ore
                                && Skills.Contains(Skill.Miner))
                            {
                                Resources[ResourceType.Ore] += tile.GatherResource(ResourceType.Ore);
                                SetBusy(5);
                            }
                            else if (tile.m_resource == ResourceType.Coal
                                && Skills.Contains(Skill.Miner))
                            {
                                Resources[ResourceType.Coal] += tile.GatherResource(ResourceType.Coal);
                                SetBusy(5);
                            }
                        }
                        break;
                    case action.Store:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);

                            if (CurrentBuilding.m_buildingtype == BuildingType.Storage)
                            {
                                SetBusy(1);
                                foreach (var item in Resources)
                                {
                                    Map.GlobalResources[item.Key] += item.Value;
                                    Resources[item.Key] = 0;
                                }
                            }
                        }
                        break;
                    case action.Move:
                        if(PathID >0)
                        {
                            if(PathFinder.Paths[PathID].isPathFound
                                && PathFinder.Paths[PathID].FoundPath.Count >0)
                            {
                                SetBusy(1);
                                currentMapPos = PathFinder.Paths[PathID].FoundPath[0].MapPos;
                                transform.position = Map.CurrentMap.getTilePos(currentMapPos);

                                PathFinder.Paths[PathID].FoundPath.RemoveAt(0);
                            }

                        }
                        break;
                    case action.Smelt:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);

                            if (CurrentBuilding.m_buildingtype == BuildingType.Smelter
                                && Skills.Contains(Skill.Labourer)
                                && Map.GlobalResources[ResourceType.Ore] > 0
                                && Map.GlobalResources[ResourceType.Coal] > 0)
                            {
                                SetBusy(5);
                                Map.GlobalResources[ResourceType.Ore]--;
                                Map.GlobalResources[ResourceType.Coal]--;

                                Resources[ResourceType.Iron]++;
                            }
                        }
                        break;
                    case action.Quarry:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);

                            if (CurrentBuilding.m_buildingtype == BuildingType.Quarry
                                && Skills.Contains(Skill.Labourer))
                            {
                                SetBusy(5);
                                Resources[ResourceType.Stone]++;
                            }
                        }
                        break;
                    case action.SawWood:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);

                            if (CurrentBuilding.m_buildingtype == BuildingType.Sawmill
                                && Skills.Contains(Skill.Labourer)
                                 && Map.GlobalResources[ResourceType.Timber] > 0)
                            {
                                SetBusy(10);
                                Map.GlobalResources[ResourceType.Timber]--;
                                Resources[ResourceType.Wood]++;
                            }
                        }
                        break;
                    case action.MakeTool://may drop
                        //check for resource 
                        //check the Skill
                        //set busy time
                        break;
                    case action.Combat://may drop
                        //check the Skill
                        //Check chance
                        //set busy time
                        //Kill person?
                        break;
                    default:
                        break;
                }
            }
        }
	}



    public void Move(IVec2 toLocation, actfunc Callback)
    {

        if(PathFinder.Paths.ContainsKey(PathID))
        {
            PathFinder.Paths.Remove(PathID);
        }

        currentstate = State.move;

        PathID = finder.GetPath(currentMapPos, toLocation, 0.01f,this);
       
    }

    public void SetBusy(int timeUnits)
    {
        BusyTime = timeUnits;
    }

    

    void OnDrawGizmos()
    {
        if (CurrentMap)
        {
            if (PathID > 0)
            {
                if (PathFinder.Paths[PathID].isPathFound)
                {
                    var foundpath = PathFinder.Paths[PathID].FoundPath;

                    for (int idx = 0; idx < foundpath.Count; ++idx)
                    {
                        IVec2 LineStart, LineEnd;

                        LineStart = foundpath[idx].MapPos;
                        if (foundpath[idx].NextNode != null)
                        {
                            LineEnd = foundpath[idx].NextNode.MapPos;

                            Vector3 realstart, realEnd;
                            realstart = CurrentMap.getTilePos(LineStart.x, LineStart.y);
                            realEnd = CurrentMap.getTilePos(LineEnd.x, LineEnd.y);
                            // realstart.y += 10;
                            //realEnd.y += 10;
                            Gizmos.DrawLine(realstart, realEnd);

                        }
                    }
                }
            }
        }
    }

    private void ResetResources()
    {
        Resources.Clear();
        for (ResourceType i = ResourceType.Wood; i < ResourceType.NumOfResourcetypes; i++)
        {
            Resources.Add(i, 0);
        }
    }
}
