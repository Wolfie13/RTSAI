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

public delegate void actfunc();

public class Person : MonoBehaviour {
    
    public List<Skill> Skills = new List<Skill>();
    [HideInInspector]
    public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();

    public List<Action> ToDoList = new List<Action>();
    
	public IVec2 currentMapPos = new IVec2 ();

    public int BusyTime = 0;

    public int teamID;

    public Material BusyTexture, FreeTexture;

	// Use this for initialization
	void Start () {
		FreeTexture = this.renderer.material;
        ResetResources();
        Skills.Add(Skill.Labourer);
	}
	
    public void SetMapPosition(IVec2 mapPos)
    {
        currentMapPos = mapPos;

        transform.position = Map.getTileCenterPos(mapPos);
    }

	// Update is called once per frame
	public void personTick () {
        if(BusyTime >0)
        {
            renderer.material = BusyTexture;
            BusyTime--;
            return;
        }
        renderer.material = FreeTexture;
        if(ToDoList.Count >0)
        {
			Debug.Log(ToDoList[0].GetType().ToString());
			Action.ActionResult result = ToDoList[0].actionTick(this);

			switch(result) {
			case Action.ActionResult.CONTINUE:
				//Do nothing
				break;

			case Action.ActionResult.FAIL:
				//Increment failed action counter
				Map.CurrentMap.GetTeamData(this.teamID).failedOrders++;
				goto case Action.ActionResult.SUCCESS;
			case Action.ActionResult.SUCCESS:
				ToDoList.RemoveAt(0);
				break;
			}
        }
	}

    public void SetBusy(int timeUnits)
    {
        BusyTime = timeUnits;
    }

/*    void OnDrawGizmos()
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
    }*/

    private void ResetResources()
    {
        Resources.Clear();
        for (ResourceType i = ResourceType.Wood; i < ResourceType.NumOfResourcetypes; i++)
        {
            Resources.Add(i, 0);
        }
    }
}
