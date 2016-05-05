using UnityEngine;
using System.Collections;
using System.Collections.Generic;


struct  Node
{
    public Vector2 MapPos;
    //public Node? NextNode;
    //public Node? PrevNode;
};

struct path
{
    public List<Node> FoundPath;
    public bool isPathFound;

};



public class PathFinder : MonoBehaviour {

    // this is a bridge class to call the Path Finding Functions 
    AStar AStar = new AStar();

   public static Map CurrentMap = null;

    public static Dictionary<uint,path> Paths;

    private uint count = 0;

    void Start () {
        CurrentMap = GetComponent<Map>();
	}



    public uint GetPath(Vector2 MapPosStart, Vector2 MapPosEnd, int Maxsteps = int.MaxValue, float TimePerframe = float.MaxValue)
    {
        uint ID = count;
        ++count;

        path temp = new path();
        temp.isPathFound = false;
        Paths.Add(ID,temp);

        StartCoroutine(AStar.GetPath(MapPosStart, MapPosEnd, Maxsteps, TimePerframe, ID));

        return ID;
    }




}
