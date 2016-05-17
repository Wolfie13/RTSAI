using UnityEngine;
using System.Collections;

public class pathPlannerTester : MonoBehaviour {


    public Vector2 startpos, endpos;

    public IVec2 istartpos = new IVec2(), iendpos = new IVec2();

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
            pathID = finder.GetPath(istartpos, iendpos);
        }
                   
	}

    void OnDrawGizmos()
    {
        if (CurrentMap)
        {
            Gizmos.color = Color.blue;
			Gizmos.DrawSphere(Map.getTilePos(istartpos.x, istartpos.y), 10);
            Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(Map.getTilePos(iendpos.x, iendpos.y), 10);
            Gizmos.color = Color.red;
            if (pathID > 0)
            {
                if (PathFinder.Paths[pathID].isPathFound)
                {
                    var foundpath = PathFinder.Paths[pathID].FoundPath;

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
}
