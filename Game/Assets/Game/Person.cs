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

    public int teamID;

    public Material BusyTexture, FreeTexture;

	// Use this for initialization
	void Start () {
        finder = GameObject.FindGameObjectWithTag("Map").GetComponent<PathFinder>();

        ResetResources();
        Skills.Add(Skill.Labourer);
	}
	
    public void SetMapPosition(IVec2 mapPos)
    {
        currentMapPos = mapPos;

        transform.position = Map.getTilePos(mapPos);
    }

	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if(time > Map.TimeUnit)
        {
            time = 0;
            if(BusyTime >0)
            {
                renderer.material = BusyTexture;
                BusyTime--;
                return;
            }
            renderer.material = FreeTexture;
            if(ToDoList.Count >0)
            {
                PlayerData CurrentTeamData = Map.CurrentMap.GetTeamData(teamID);
                switch (ToDoList[0])
                {
                    case action.Family:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);
                            //check for another person 
                            var other = CurrentBuilding.GetNonBusyPersonInBuilding();
                            if (CurrentBuilding.teamID == teamID && other != null && other.teamID == teamID)
                            {
                                SetBusy(20);
                                CurrentBuilding.GetNonBusyPersonInBuilding().SetBusy(20);
                                if (CurrentBuilding.m_buildingtype == BuildingType.turfHut)
                                {
                                    Map.CurrentMap.AddPerson(currentMapPos,teamID);
                                }
                                else if( CurrentBuilding.m_buildingtype == BuildingType.House)
                                {
                                    Map.CurrentMap.AddPerson(currentMapPos, teamID);
                                    Map.CurrentMap.AddPerson(currentMapPos, teamID);
                                }
                            }
                        }

                        break;
                    case action.Educate:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);

                            var other = CurrentBuilding.GetNonBusyPersonInBuilding();

                            if (Skills.Contains(Skill.Rifleman)
                                && CurrentBuilding.m_buildingtype == BuildingType.Barracks
                                && CurrentBuilding.teamID == teamID
                                && other != null
                                && other.teamID == teamID)
                            {
                           
                                SetBusy(30);
                                other.SetBusy(30);
                                other.Skills.Add(Skill.Rifleman);
                            }
                        }
                        else
                        {
                            Person other = Map.CurrentMap.GetNonBusyPersonAt(currentMapPos);
                            if (other == null || other.teamID != teamID)
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

                            if (CurrentBuilding.m_buildingtype == BuildingType.School
                                && CurrentBuilding.teamID == teamID)
                            {
                                var other = CurrentBuilding.GetNonBusyPersonInBuilding();
                                while (other != null && other.teamID == teamID)
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
                                && CurrentBuilding.m_buildingtype == BuildingType.Mine
                                && CurrentBuilding.teamID == teamID)
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

                            if (CurrentBuilding.m_buildingtype == BuildingType.Storage
                                && CurrentBuilding.teamID == teamID)
                            {
                                SetBusy(1);
                                foreach (var item in Resources)
                                {
                                    Map.CurrentMap.GetTeamData(teamID).Resources[item.Key] += item.Value;
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
                                currentMapPos = PathFinder.Paths[PathID].FoundPath[0].MapPos;
                                transform.position = Map.getTilePos(currentMapPos);

                                PathFinder.Paths[PathID].FoundPath.RemoveAt(0);
                            }

                        }
                        break;
                    case action.Smelt:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);

                            if (CurrentBuilding.m_buildingtype == BuildingType.Smelter
                                && CurrentBuilding.teamID == teamID
                                && Skills.Contains(Skill.Labourer)
                                && CurrentTeamData.Resources[ResourceType.Ore] > 0
                                && CurrentTeamData.Resources[ResourceType.Coal] > 0)
                            {
                                SetBusy(5);
                                CurrentTeamData.Resources[ResourceType.Ore]--;
                                CurrentTeamData.Resources[ResourceType.Coal]--;

                                Resources[ResourceType.Iron]++;
                            }
                        }
                        break;
                    case action.Quarry:
                        if (Map.CurrentMap.getObject(currentMapPos) is Building)
                        {
                            Building CurrentBuilding = (Building)Map.CurrentMap.getObject(currentMapPos);

                            if (CurrentBuilding.m_buildingtype == BuildingType.Quarry
                                && CurrentBuilding.teamID == teamID
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
                                && CurrentBuilding.teamID == teamID
                                && Skills.Contains(Skill.Labourer)
                                && CurrentTeamData.Resources[ResourceType.Timber] > 0)
                            {
                                SetBusy(10);
                                CurrentTeamData.Resources[ResourceType.Timber]--;
                                Resources[ResourceType.Wood]++;
                            }
                        }
                        break;
                    case action.MakeTool://may drop
                        //check for resource 
                        //check the Skill
                        //set busy time
                        break;
                    case action.Combat:
                        if (Skills.Contains(Skill.Rifleman))
                        {
                            List<Person> others = new List<Person>();
                            for(IVec2 offset = new IVec2(-5,-5); offset.x < 6;++offset.x)
                            {
                                for(offset.y = -5; offset.y < 6;++offset.y)
                                {
                                    if(offset.magnitude() <= 5)
                                    {
                                        others.AddRange(Map.CurrentMap.GetPeopleAt(currentMapPos + offset));
                                    }
                                }
                            }
                            Person other = null;
                            foreach (var item in others)
                            {
                                if(item.teamID != teamID)
                                {
                                    other = item;
                                    break;
                                }
                            }
                            if(other)
                            {
                                if(other.Skills.Contains(Skill.Rifleman) && Random.Range(0,100) < 70)
                                {
                                    Map.CurrentMap.KillPerson(this);
                                }
                                else
                                {
                                    Map.CurrentMap.KillPerson(other);
                                    SetBusy(1);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
                ToDoList.RemoveAt(0);
            }
        }
	}



    public void Move(IVec2 toLocation)
    {

        if(PathFinder.Paths.ContainsKey(PathID))
        {
            PathFinder.Paths.Remove(PathID);
        }

        currentstate = State.move;

        PathID = finder.GetPath(currentMapPos, toLocation, 10,this);
       
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
                            realstart = Map.getTilePos(LineStart.x, LineStart.y);
							realEnd = Map.getTilePos(LineEnd.x, LineEnd.y);
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
