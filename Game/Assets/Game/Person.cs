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

public struct DoAction
{
    DoAction(action a, actfunc cb)
    {
        todo = a;
        act = cb;
    }
    action todo;
    actfunc act;
}

public class Person : MonoBehaviour {

    [HideInInspector]
    public List<Skill> Skills = new List<Skill>();
    [HideInInspector]
    public Dictionary<Resources, int> Resources = new Dictionary<Resources, int>();

    public List<DoAction> ToDoList = new List<DoAction>();
    
    public IVec2 currentMapPos = new IVec2();

    public State currentstate = State.Idle;

    Map CurrentMap = null;
    PathFinder finder = null;

    uint PathID = 0;
    float actionStartTime = 0;
  
    int actiontime = 0;

	// Use this for initialization
	void Start () {
        finder = GameObject.FindGameObjectWithTag("Map").GetComponent<PathFinder>();

        if (Map.CurrentMap)
            currentMapPos = Map.CurrentMap.getTileFromPos(transform.position);
	}
	
	// Update is called once per frame
	void Update () {


       
	
	}



    public void Move(IVec2 toLocation, actfunc Callback)
    {

        if(PathFinder.Paths.ContainsKey(PathID))
        {
            PathFinder.Paths.Remove(PathID);
        }

        currentstate = State.move;

        PathID = finder.GetPath(currentMapPos, toLocation, 0.01f);

        actionStartTime = Time.time;
       
    }

    public void SetBusy(int timeUnits, actfunc waitcallback)
    {
        currentstate = State.action;
        actionStartTime = Time.time;
        actiontime = timeUnits;
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
