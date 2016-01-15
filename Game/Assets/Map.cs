using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	private MapObject[][] entities;
	private char[][] mapTiles;
	private int sizeX, sizeY;

	private bool dirty = false;

	public MapObject getObject (int x, int y)
	{
		//TODO: bounds check
		return entities [x] [y];
	}

	public char getTile(int x, int y)
	{
		return mapTiles [x] [y];
	}

	public char setTile(int x, int y, char tile)
	{
		//TODO: Bounds checking again!
		mapTiles [x] [y] = tile;
		dirty = true;
	}

	// Use this for initialization
	void Start () {
	
	}

	void load(string filename)
	{
		//load mapsize from file
		//initialize entity array
		//init maptiles array
		//fill maptiles from file
		//close file
		//generate resource entities
	}
	
	// Update is called once per frame
	void Update () {
		if (dirty) {
			//regen map mesh
			dirty = false;
		} 
	}
}
