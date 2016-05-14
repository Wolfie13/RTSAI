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

    [HideInInspector]
    public List<Skill> Skills = new List<Skill>();
    [HideInInspector]
    public Dictionary<Resources, int> Resources = new Dictionary<Resources, int>();

    public List<action> ToDoList = new List<action>();
    
    public IVec2 currentMapPos = new IVec2();

    public State currentstate = State.Idle;

    Map CurrentMap = null;
    PathFinder finder = null;

    uint PathID = 0;
    public int BusyTime = 0;
  
    float time = 0;

	// Use this for initialization
	void Start () {
        finder = GameObject.FindGameObjectWithTag("Map").GetComponent<PathFinder>();

        if (Map.CurrentMap)
            currentMapPos = Map.CurrentMap.getTileFromPos(transform.position);
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
                        //check for Tree resource 
                        //check the Skill
                        //set busy time
                        break;
                    case action.Mine:
                        //check for resource 
                        //check the Skill
                        //set busy time
                        break;
                    case action.Store:
                        //check for resource 
                        //set busy time
                        break;
                    case action.Move:
                        //move
                        //set busy time
                        break;
                    case action.Smelt:
                        //check for resource 
                        //check the Skill
                        //set busy time
                        break;
                    case action.Quarry:
                        //check the Skill
                        //set busy time
                        break;
                    case action.SawWood:
                        //check for resource 
                        //check the Skill
                        //set busy time
                        break;
                    case action.MakeTool:
                        //check for resource 
                        //check the Skill
                        //set busy time
                        break;
                    case action.Combat:
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

        PathID = finder.GetPath(currentMapPos, toLocation, 0.01f);
       
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
}
