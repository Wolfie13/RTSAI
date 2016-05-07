using UnityEngine;
using System.Collections;

public class pathPlannerTester : MonoBehaviour {


    public Vector2 startpos, endpos;

    public ivec2 istartpos = new ivec2(), iendpos = new ivec2();

    private Map CurrentMap = null;
    private PathFinder finder = null;

    uint pathID = 0;

	// Use this for initialization
	void Start () {
        CurrentMap = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
        finder = GetComponent<PathFinder>();

        istartpos.x = Mathf.FloorToInt(startpos.x);
        istartpos.y = Mathf.FloorToInt(startpos.y);

        iendpos.x = Mathf.FloorToInt(endpos.x);
        iendpos.y = Mathf.FloorToInt(endpos.y);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(pathID == 0)
        {
            pathID = finder.GetPath(istartpos, iendpos, 16);
        }
                   
	}

    void OnDrawGizmos()
    {
       if(pathID > 0)
        {
           if(PathFinder.Paths[pathID].isPathFound)
            {
               var foundpath = PathFinder.Paths[pathID].FoundPath;

                for(int idx = 0; idx < foundpath.Count; ++idx)
                {
                    ivec2 LineStart, LineEnd;

                    LineStart = foundpath[idx].MapPos;
                    if (foundpath[idx].NextNode != null) ;
                    {
                        LineEnd = foundpath[idx].NextNode.MapPos;

                       Vector3 realstart, realEnd;
                       realstart = CurrentMap.getTilePos(LineStart.x, LineStart.y);
                       realEnd = CurrentMap.getTilePos(LineEnd.x, LineEnd.y);
                       realstart.y += 10;
                       realEnd.y += 10;
                       Gizmos.color = Color.red;
                       Gizmos.DrawLine(realstart, realEnd);

                    }
                }
            }
        }
    }
}
