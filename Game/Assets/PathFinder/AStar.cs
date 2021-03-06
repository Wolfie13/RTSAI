﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStar {

    struct AStarNodes
    {
       public Node NodeInfo;
       public float DistanceGone;
       public float Distance2Go;
    }

    public path GetPath(IVec2 MapPosStart, IVec2 MapPosEnd)
    {
        List<Node> result = new List<Node>();

        List<AStarNodes> openQueue = new List<AStarNodes>();
        
        List<AStarNodes> CloseList = new List<AStarNodes>();

        AStarNodes startNode = new AStarNodes();
        Node EndNode = null;

        startNode.NodeInfo = new Node();
        startNode.NodeInfo.MapPos = MapPosStart;
        startNode.NodeInfo.MapSymbol = Map.CurrentMap.getTile(MapPosStart.x, MapPosStart.y);
        startNode.DistanceGone = 0;
        startNode.Distance2Go = GetDirectDistance2End(MapPosStart, MapPosEnd);
        openQueue.Add(startNode);
        CloseList.Add(startNode);

        while (openQueue.Count > 0)
        {
            var currentNode = openQueue[0];

            openQueue.RemoveAt(0);

			if(currentNode.NodeInfo.MapPos == MapPosEnd)
            {
                EndNode = currentNode.NodeInfo;
                break;
            }

			var newNodes = GetNextPositions(currentNode, MapPosEnd);

            foreach (var item in newNodes)
            {
                if (isValid(item, ref CloseList))
                {
                    openQueue.Add(item);
                    CloseList.Add(item);
                }
            }


            openQueue.Sort((AStarNodes a, AStarNodes b) => (a.Distance2Go + a.DistanceGone).CompareTo(b.Distance2Go + b.DistanceGone));

        }

        while(EndNode != null)
        {
            if(EndNode.PrevNode != null)
                EndNode.PrevNode.NextNode = EndNode;

            result.Insert(0, EndNode);
            
            EndNode = EndNode.PrevNode;
        }

//        Debug.Log("AStarStopped path size: " + result.Count);

        //output
        {
            path theWay = new path();
            theWay.FoundPath = result;
            theWay.isPathFound = result.Count > 0;
			return theWay;
        }
    }



    private float GetDirectDistance2End(IVec2 point, IVec2 end)
    {
        return (point - end).magnitude();
    }

	private List<AStarNodes> GetNextPositions(AStarNodes CurrentNode, IVec2 MapPosEnd)
    {
        List<AStarNodes> NextPositions = new List<AStarNodes>();

        for (IVec2 offset = new IVec2(-1, -1); offset.x <= 1; ++offset.x)
        {

            for (offset.y = -1; offset.y <= 1; ++offset.y)
            {
                IVec2 newPos = CurrentNode.NodeInfo.MapPos + offset;

                if(newPos.x <0 || 
                    newPos.x > Map.CurrentMap.sizeX)
                     break;

                if (newPos.y < 0 || newPos == CurrentNode.NodeInfo.MapPos ||
                    newPos.y > Map.CurrentMap.sizeY ||
                    offset == new IVec2(0,0))
                    continue;

               
                if(Map.Trees.Contains(Map.CurrentMap.getTile(newPos.x, newPos.y)) ||
                   Map.Terrain.Contains(Map.CurrentMap.getTile(newPos.x, newPos.y)))
                {
                    AStarNodes newNode = new AStarNodes();

                    newNode.DistanceGone = CurrentNode.DistanceGone + offset.magnitude();
                    //newNode.NodeInfo.MapSymbol = PathFinder.CurrentMap.getTile(newPos.x, newPos.y)
                    newNode.NodeInfo = new Node();
                    newNode.NodeInfo.MapPos = newPos;
                    newNode.NodeInfo.PrevNode = CurrentNode.NodeInfo;
					newNode.Distance2Go = GetDirectDistance2End(newPos, MapPosEnd);

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
