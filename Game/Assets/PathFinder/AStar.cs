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

    private Dictionary<uint, ivec2> EndPos = new Dictionary<uint,ivec2>();

    public IEnumerator GetPath(ivec2 MapPosStart, ivec2 MapPosEnd, int Maxsteps, float TimePerframe, uint ID)
    {
        List<Node> result = new List<Node>();

        List<AStarNodes> openQueue = new List<AStarNodes>();
        
        List<AStarNodes> CloseList = new List<AStarNodes>();

        AStarNodes startNode = new AStarNodes();
        Node EndNode = null;

        System.Diagnostics.Stopwatch myTimer = new System.Diagnostics.Stopwatch();

        myTimer.Reset();

        EndPos.Add(ID, MapPosEnd);
        startNode.NodeInfo = new Node();
        startNode.NodeInfo.MapPos = MapPosStart;
        startNode.NodeInfo.MapSymbol = PathFinder.CurrentMap.getTile(MapPosStart.x, MapPosStart.y);
        startNode.DistanceGone = 0;
        startNode.Distance2Go = GetDirectDistance2End(MapPosStart,ID);
        openQueue.Add(startNode);
        CloseList.Add(startNode);

        myTimer.Start();

        while (openQueue.Count > 0)
        {
            var currentNode = openQueue[0];

            openQueue.RemoveAt(0);

            // do stuff and Yeild on Timeperframe
            if(myTimer.ElapsedMilliseconds > TimePerframe)
            {
                myTimer.Reset();
                yield return true;
                myTimer.Start();

            }
            if(currentNode.NodeInfo.MapPos == EndPos[ID])
            {
                EndNode = currentNode.NodeInfo;
                break;
            }

            var newNodes = GetNextPositions(currentNode, ID);

            foreach (var item in newNodes)
            {
                if (isValid(item, ref CloseList))
                {
                    openQueue.Add(item);
                    CloseList.Add(item);
                }
            }


            openQueue.Sort((AStarNodes a, AStarNodes b) => (a.Distance2Go + a.DistanceGone).CompareTo(b.Distance2Go + b.DistanceGone));


            //Debug.Log("OpenQueue size: " + openQueue.Count);

            //Debug.Log("close list size: " + CloseList.Count);
        }

        while(EndNode != null)
        {
            if(EndNode.PrevNode != null)
                EndNode.PrevNode.NextNode = EndNode;

            result.Insert(0, EndNode);
            
            EndNode = EndNode.PrevNode;
        }

        Debug.Log("AStarStopped path size: " + result.Count);

        //output
        if (PathFinder.Paths.ContainsKey(ID))
        {
            path theWay = new path();
            theWay.FoundPath = result;
            theWay.isPathFound = result.Count > 0;
            PathFinder.Paths[ID] = theWay;
        }
    }



    private float GetDirectDistance2End(ivec2 point, uint ID)
    {
        return (point - EndPos[ID]).magnitude();
    }

    private List<AStarNodes> GetNextPositions(AStarNodes CurrentNode, uint ID)
    {
        List<AStarNodes> NextPositions = new List<AStarNodes>();

       
        ivec2 offset = new ivec2(-1,-1);
        for (; offset.x <= 1; ++offset.x )
        {

            for (offset.y = -1; offset.y <= 1; ++offset.y)
            {
                ivec2 newPos = CurrentNode.NodeInfo.MapPos + offset;

                if(newPos.x <0 || 
                    newPos.x > PathFinder.CurrentMap.sizeX)
                     break;

                if (newPos.y < 0 || newPos == CurrentNode.NodeInfo.MapPos ||
                    newPos.y > PathFinder.CurrentMap.sizeY)
                    continue;

               
                if(PathFinder.CurrentMap.getObject(newPos.x, newPos.y) != null &&
                   PathFinder.CurrentMap.getObject(newPos.x, newPos.y).isTraversable())
                {
                    AStarNodes newNode = new AStarNodes();

                    newNode.DistanceGone = CurrentNode.DistanceGone + offset.magnitude();
                    //newNode.NodeInfo.MapSymbol = PathFinder.CurrentMap.getTile(newPos.x, newPos.y)
                    newNode.NodeInfo = new Node();
                    newNode.NodeInfo.MapPos = newPos;
                    newNode.NodeInfo.PrevNode = CurrentNode.NodeInfo;
                    newNode.Distance2Go = GetDirectDistance2End(newPos, ID);

                    NextPositions.Add(newNode);
                }

            }

        }

            return NextPositions;

    }

    private bool isValid(AStarNodes testNode, ref List<AStarNodes> CloseList)
    {
        bool found = false;

        for(int idx = CloseList.Count-1; idx>=0; --idx)
        {
            if(CloseList[idx].NodeInfo.MapPos == testNode.NodeInfo.MapPos)
            {
                if(CloseList[idx].DistanceGone > testNode.DistanceGone)
                {
                    CloseList.RemoveAt(idx);
                    break;
                }
                found = true;
            }
        }
       
        return !found;
    }
}
