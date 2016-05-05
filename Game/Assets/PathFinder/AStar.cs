using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {

    struct AStarNodes
    {
       public Node NodeInfo;
       public float DistanceGone;
       public float Distance2Go;

    }

    public IEnumerator GetPath(Vector2 MapPosStart, Vector2 MapPosEnd, int Maxsteps, float TimePerframe, uint ID)
    {
        List<Node> result = new List<Node>(); 
        List<AStarNodes> OpenList = new List<AStarNodes>();
        List<AStarNodes> CloseList = new List<AStarNodes>();

        AStarNodes startNode = new AStarNodes();

        startNode.NodeInfo.MapPos = MapPosStart;
        startNode.DistanceGone = 0;
        startNode.Distance2Go = GetDirectDistance(MapPosStart,MapPosEnd);
        OpenList.Add(startNode);

        while(OpenList.Count >0)
        {
            // do stuff and Yeild on Timeperframe
        }






        //output
        path theWay = new path();
        theWay.FoundPath = result;
        theWay.isPathFound = true;
        PathFinder.Paths[ID] = theWay;

        yield return true;
    }



    private float GetDirectDistance(Vector2 pointA, Vector2 pointB)
    {
        return (pointA - pointB).magnitude;
    }


}
