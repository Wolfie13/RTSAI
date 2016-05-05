﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Node
{
    public ivec2 MapPos;
    public Node NextNode = null;
    public Node PrevNode = null;
    public char MapSymbol;
};

public struct path
{
    public List<Node> FoundPath;
    public bool isPathFound;
};


public class ivec2
{
    public ivec2(){x = 0; y = 0;}
    public ivec2(int newx, int newy) { x = newx; y = newy; }
    public ivec2(ivec2 newVec) { x = newVec.x; y = newVec.y; }

    public static ivec2 operator +(ivec2 a, ivec2 b) { return new ivec2(a.x + b.y, a.y + b.y); }
    public static ivec2 operator -(ivec2 a, ivec2 b) { return new ivec2(a.x - b.y, a.y - b.y); }
    public static ivec2 operator *(ivec2 a, int b) { return new ivec2(a.x * b, a.y * b); }

    public float magnitude() { return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2)); }

    public int x, y;
}


public class PathFinder : MonoBehaviour {

    // this is a bridge class to call the Path Finding Functions 
    AStar AStar = new AStar();

   public static Map CurrentMap = null;

    public static Dictionary<uint,path> Paths = new Dictionary<uint,path>();

    private uint count = 1;

    void Start () {
        CurrentMap = GetComponent<Map>();
	}



    public uint GetPath(ivec2 MapPosStart, ivec2 MapPosEnd, int Maxsteps = int.MaxValue, float TimePerframe = float.MaxValue)
    {
        if(!CurrentMap)
            return 0;
        
        // returns 0 if failed returns an ID for you path if success
        uint ID = count;
        ++count;

        path temp = new path();
        temp.isPathFound = false;
        Paths.Add(ID,temp);

        StartCoroutine(AStar.GetPath(MapPosStart, MapPosEnd, Maxsteps, TimePerframe, ID));

        return ID;
    }




}
