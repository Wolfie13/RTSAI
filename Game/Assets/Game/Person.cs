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
    int BusyTime = 0;
  
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
                        //check for another person 
                        //check the Building
                        //set busy time
                        break;
                    case action.Educate:
                        //check for another person 
                        //check the Building
                        //set busy time
                        break;
                    case action.Train:
                        //check for another person 
                        //check the Building
                        //set busy time
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
