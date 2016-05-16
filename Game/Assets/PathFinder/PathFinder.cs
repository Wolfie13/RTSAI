using UnityEngine;
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


public class IVec2
{
    public IVec2(){x = 0; y = 0;}
    public IVec2(int newx, int newy) { x = newx; y = newy; }
    public IVec2(IVec2 newVec) { x = newVec.x; y = newVec.y; }

    public static IVec2 operator +(IVec2 a, IVec2 b) { return new IVec2(a.x + b.x, a.y + b.y); }
    public static IVec2 operator -(IVec2 a, IVec2 b) { return new IVec2(a.x - b.x, a.y - b.y); }
    public static IVec2 operator *(IVec2 a, int b) { return new IVec2(a.x * b, a.y * b); }
    public static bool operator ==(IVec2 a, IVec2 b) { return (a.x==b.x && a.y == b.y);}
    public static bool operator !=(IVec2 a, IVec2 b) { return !(a==b); }

    public float magnitude() { return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2)); }

    public override string ToString() { return "(" + x + ", " + y + ")"; }

    public int x, y;

	public override bool Equals(System.Object o)
	{
		return o.GetType ().Equals (this.GetType ()) && (o as IVec2) == this;
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}
}


public class PathFinder : MonoBehaviour {

    // this is a bridge class to call the Path Finding Functions 
   static private AStar AStar = new AStar();

    public static Dictionary<uint,path> Paths = new Dictionary<uint,path>();

    private static uint count = 1;

    void Start () {
        DontDestroyOnLoad(this);
	}


    public uint GetPath(IVec2 MapPosStart, IVec2 MapPosEnd, float TimePerframe, Person jim)
    {
        return GetPath(MapPosStart, MapPosEnd, TimePerframe, int.MaxValue, jim);
    }

    public uint GetPath(IVec2 MapPosStart, IVec2 MapPosEnd, Person jim )
    {
        return GetPath(MapPosStart, MapPosEnd, float.MaxValue, int.MaxValue, jim);
    }

	public uint GetPath(IVec2 MapPosStart, IVec2 MapPosEnd)
	{
		return GetPath (MapPosStart, MapPosEnd, float.MaxValue, int.MaxValue, null);
	}

    public uint GetPath(IVec2 MapPosStart, IVec2 MapPosEnd, float TimePerframe, int Maxsteps, Person jim)
    {
        if(!Map.CurrentMap)
            return 0;
        
        // returns 0 if failed returns an ID for you path if success
        uint ID = count;
        ++count;

        path temp = new path();
        temp.isPathFound = false;
        Paths.Add(ID,temp);

        StartCoroutine(AStar.GetPath(MapPosStart, MapPosEnd, Maxsteps, TimePerframe, ID, jim));

        return ID;
    }




}
