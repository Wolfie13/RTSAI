using UnityEngine;
using System;
using System.Collections;

public class Map : MonoBehaviour {
	[SerializeField]
	private string MapName;
	private MapObject[,] entities;
	private char[,] mapTiles;
	private int sizeX, sizeY;

	private bool dirty = false;

	public MapObject getObject (int x, int y)
	{
		//TODO: bounds check
		return entities [x, y];
	}

	public char getTile(int x, int y)
	{
		return mapTiles [x, y];
	}

	public void setTile(int x, int y, char tile)
	{
		//TODO: Bounds checking again!
		mapTiles [x, y] = tile;
		dirty = true;
	}

	// Use this for initialization
	void Start () {
		load (MapName);
	}

	void load(string filename)
	{
		// Read the file
		System.IO.StreamReader file = 
			new System.IO.StreamReader(filename);

		//load mapsize from file
		//type octile
		file.ReadLine ();
		//height
		this.sizeY = Int32.Parse (file.ReadLine ().Split (' ') [1]);
		//width
		this.sizeX = Int32.Parse (file.ReadLine ().Split (' ') [1]);
		//"map"
		file.ReadLine ();

		//init maptiles array
		this.mapTiles = new char[sizeX, sizeY];


		//initialize entity array
		this.entities = new MapObject[sizeX, sizeY];


		//fill maptiles from file
		string line;
		int lineCount = 0;
		while((line = file.ReadLine()) != null)
		{
			for (int i = 0; i < line.Length; i++)
			{
				mapTiles[i, lineCount] = line[i];
			}

			lineCount++;
		}

		//close file
		file.Close();

		//generate resource entities

	}

	void generateMesh()
	{
		//iterate over every map tile
		//push some verts to a buffer
		//write the buffer into the meshfilter
	}
	
	// Update is called once per frame
	void Update () {
		if (dirty) {
			generateMesh();
			dirty = false;
		} 
	}
}
