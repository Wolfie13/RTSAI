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

    private Dictionary<uint, ivec2> EndPos;

    public IEnumerator GetPath(ivec2 MapPosStart, ivec2 MapPosEnd, int Maxsteps, float TimePerframe, uint ID)
    {
        List<Node> result = new List<Node>(); 
        List<AStarNodes> OpenList = new List<AStarNodes>();

       // SortedList<float,AStarNodes> openQueue = new SortedList<float,AStarNodes>()
        
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


        ivec2 offset = new ivec2(-1,-1);
        for (; offset.x <= 1; ++offset.x )
        {
            for (; offset.y <= 1; ++offset.y)
            {
                ivec2 newPos = CurrentNode.NodeInfo.MapPos + offset;

                if(newPos.x <0 ||
                    newPos.x > PathFinder.CurrentMap.sizeX)
                     break;

                if (newPos.y < 0 ||
                    newPos.y > PathFinder.CurrentMap.sizeY)
                    continue;                    

                if(Map.Terrain.Contains(PathFinder.CurrentMap.getTile(newPos.x, newPos.y)))
                {
                    AStarNodes newNode = new AStarNodes();

                    newNode.DistanceGone = CurrentNode.DistanceGone + offset.magnitude();
                    //newNode.NodeInfo.MapSymbol = PathFinder.CurrentMap.getTile(newPos.x, newPos.y)
                    newNode.NodeInfo.MapPos = newPos;
                    newNode.NodeInfo.PrevNode = CurrentNode.NodeInfo;
                    //newNode.Distance2Go

                    //NextPositions.Add
                }

            }

        }

            return NextPositions;

    }
}
