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

public delegate void WaitOver();


public class Person : MonoBehaviour {

    [HideInInspector]
    public List<Skill> Skills = new List<Skill>();
    [HideInInspector]
    public Dictionary<Resources, int> Resources = new Dictionary<Resources, int>();
    
    ivec2 currentMapPos = new ivec2();

    State currentstate = State.Idle;

    Map CurrentMap = null;
    PathFinder finder = null;

    uint PathID = 0;
    float actionStartTime = 0;
    WaitOver MoveCallback;


    int actiontime = 0;
    WaitOver busyCallback;

	// Use this for initialization
	void Start () {
        CurrentMap = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
        finder = GameObject.FindGameObjectWithTag("Map").GetComponent<PathFinder>();

        if (CurrentMap)
            currentMapPos = CurrentMap.getTileFromPos(transform.position);
	}
	
	// Update is called once per frame
	void Update () {

        if (!CurrentMap)
            return;

        switch (currentstate)
        {
            case State.Idle:
                break;
            case State.action:
                float actiontimeTime = (Time.time - actionStartTime) / CurrentMap.TimeUnit;

                if(Mathf.Approximately(actiontimeTime,actiontime))
                {
                    busyCallback();
                    currentstate = State.Idle;
                }
                break;
            case State.move:
                if (PathID != 0)
                {
                    if(PathFinder.Paths[PathID].isPathFound)
                    {
                        if(PathFinder.Paths[PathID].FoundPath.Count > 0)
                        {
                           // Vector3 world_offset = new Vector3(0.5f, 0.5f, 0.5f) * MapChunk.TILE_SIZE;
                            Vector3 targetLocation = CurrentMap.getTilePos(PathFinder.Paths[PathID].FoundPath[0].MapPos);
                            float travelTime = (Time.time - actionStartTime) / CurrentMap.TimeUnit;
                            transform.position = Vector3.Lerp(CurrentMap.getTilePos(currentMapPos), targetLocation, travelTime);

                            if (travelTime>1)
                            {
                                actionStartTime = Time.time;

                                currentMapPos = PathFinder.Paths[PathID].FoundPath[0].MapPos;

                                PathFinder.Paths[PathID].FoundPath.RemoveAt(0);

                            }

                            if(PathFinder.Paths[PathID].FoundPath.Count == 0)
                            {
                                Debug.Log(currentMapPos);
                                MoveCallback();
                            }
                        }



                    }
                }
                break;
            default:
                break;
        }
	
	}



    public void Move(ivec2 toLocation, WaitOver Callback)
    {

        if(PathFinder.Paths.ContainsKey(PathID))
        {
            PathFinder.Paths.Remove(PathID);
        }

        currentstate = State.move;

        PathID = finder.GetPath(currentMapPos, toLocation, 0.01f);

        actionStartTime = Time.time;
        MoveCallback = Callback;
    }

    public void SetBusy(int timeUnits,WaitOver waitcallback)
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
                        ivec2 LineStart, LineEnd;

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
