using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class AStar {

    struct AStarNodes
    {
       public Node NodeInfo;
       public float DistanceGone;
       public float Distance2Go;
    }

    public IEnumerator GetPath(ivec2 MapPosStart, ivec2 MapPosEnd, int Maxsteps, float TimePerframe, uint ID)
    {
        List<Node> result = new List<Node>(); 
        List<AStarNodes> OpenList = new List<AStarNodes>();
        List<AStarNodes> CloseList = new List<AStarNodes>();

        AStarNodes startNode = new AStarNodes();

        Stopwatch myTimer = new Stopwatch();

        myTimer.Reset();

        startNode.NodeInfo.MapPos = MapPosStart;
        startNode.NodeInfo.MapSymbol = PathFinder.CurrentMap.getTile(MapPosStart.x, MapPosStart.y);
        startNode.DistanceGone = 0;
        startNode.Distance2Go = GetDirectDistance(MapPosStart,MapPosEnd);
        OpenList.Add(startNode);

        myTimer.Start();
        while(OpenList.Count >0)
        {
            // do stuff and Yeild on Timeperframe
            if(myTimer.ElapsedMilliseconds > TimePerframe)
            {
                myTimer.Reset();
                yield return true;
                myTimer.Start();
            }
        }

        




        //output
        path theWay = new path();
        theWay.FoundPath = result;
        theWay.isPathFound = true;
        PathFinder.Paths[ID] = theWay;

        yield return true;
    }



    private float GetDirectDistance(ivec2 pointA, ivec2 pointB)
    {
        return (pointA - pointB).magnitude();
    }

    private List<AStarNodes> GetNextPositions(AStarNodes CurrentNode)
    {
        List<AStarNodes> NextPositions = new List<AStarNodes>();



        for (int x = -1; x <= 1; ++x )
        {
            if(CurrentNode.NodeInfo.MapPos.x  + x <0 ||
               CurrentNode.NodeInfo.MapPos.x + x > PathFinder.CurrentMap.sizeX)
                continue;

            for (int y = -1; y <= 1; ++y)
            {
                if (CurrentNode.NodeInfo.MapPos.y + y < 0 ||
                    CurrentNode.NodeInfo.MapPos.y + y > PathFinder.CurrentMap.sizeY)
                    continue;


            }

        }

            return NextPositions;

    }
}
