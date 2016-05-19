using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;


public class Node
{
    public IVec2 MapPos;
    public Node NextNode = null;
    public Node PrevNode = null;
    public char MapSymbol;
};

public struct path
{
    public List<Node> FoundPath;
    public bool isPathFound{get;set;}
};


public class PathFinder {

    // this is a bridge class to call the Path Finding Functions 
   static private AStar m_AStar = new AStar();

	public static path GetPath(IVec2 MapPosStart, IVec2 MapPosEnd)
    {
		return m_AStar.GetPath(MapPosStart, MapPosEnd);
    }

	private class PathFinderWorker
	{
		public PathFinderWorker(IVec2 startPos, IVec2 endPos) {

		}
	}
}
