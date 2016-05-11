using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public enum Resource
{
    Wood,
    Timber,
    Stone,
    Ore,
    Iron,
    Coal,
    Money

}

public class Map : MonoBehaviour {
	[SerializeField]
	private string MapName;
	private MapObject[,] entities;
	private char[,] mapTiles;
	public int sizeX;
	public int sizeY;
	public Material mapMaterial;

    public const int ChunkSize = 32;


    public float TimeUnit = 1;
    private float timePassed = 0;

    //passable
    public static IList<char> Terrain = new List<char>{ '.', 'G' }.AsReadOnly();
    //out of bounds
    public static IList<char> OutBounds = new List<char> { '@', 'O' }.AsReadOnly();
    //Unpassable
    public static IList<char> Trees = new List<char> { 'T' }.AsReadOnly();
    //passable from regular terrain
    public static IList<char> swamp = new List<char> { 'S' }.AsReadOnly();
    //traversable, but not passable from terrain
    public static IList<char> water = new List<char> { 'W' }.AsReadOnly();

    public MapObject getObject(ivec2 pos) { return getObject(pos.x, pos.y); }

	public MapObject getObject (int x, int y)
	{
        if (x < 0 || x > sizeX || y < 0 || y > sizeY)
        {
            return null;
        }
		return entities [x, y];
	}

    public char getTile(ivec2 pos) { return getTile(pos.x, pos.y); }

	public char getTile(int x, int y)
	{
		if (x < 0 || x > sizeX || y < 0 || y > sizeY) {
			return '@';
		}
		return mapTiles [x, y];
	}

    public void setTile(ivec2 pos, char tile) { setTile(pos.x, pos.y, tile); }

	public void setTile(int x, int y, char tile)
	{
		if (x < 0 || x > sizeX || y < 0 || y > sizeY) {
			return;
		} else {
			mapTiles [x, y] = tile;
		}
	}

    public Vector3 getTilePos(ivec2 pos) { return getTilePos(pos.x, pos.y); }

    public Vector3 getTilePos(int x, int y)
    {
        float xPos = MapChunk.TILE_SIZE * x;
        float yPos = MapChunk.TILE_SIZE * y;

        return new Vector3(xPos, 0, yPos);
    }

    public ivec2 getTileFromPos(Vector3 worldPos)
    {
        //worldPos += new Vector3(0.5f, 0.5f, 0.5f) * MapChunk.TILE_SIZE;

        ivec2 mapPos = new ivec2(Mathf.FloorToInt(worldPos.x / MapChunk.TILE_SIZE), Mathf.FloorToInt(worldPos.z / MapChunk.TILE_SIZE));

        return mapPos;
    }

    public MapObject getobjectFromPos(Vector3 worldPos)
    {
        return getObject(getTileFromPos(worldPos));
    }

	// Use this for initialization
	void Start () {
		load (MapName);
		initChunks ();
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
                entities[i, lineCount] = new MapObject();
                entities[i, lineCount].setTile(line[i],new ivec2(i,lineCount),getTilePos(i,lineCount));
			}

			lineCount++;
		}

		//close file
		file.Close();

		//generate resource entities

	}

	public void initChunks() {
		int chunksX = this.sizeX / ChunkSize;
		int chunksY = this.sizeY / ChunkSize;

		for (int i = 0; i != chunksX; i++) {
			for (int j = 0; j != chunksY; j++) {
				GameObject newChunk = new GameObject("Chunk" + i + "," + j);
				newChunk.transform.parent = this.gameObject.transform;
				MapChunk obj = newChunk.AddComponent<MapChunk>();
				obj.chunkX = i;
				obj.chunkY = j;
				obj.parent = this;
				obj.Generate();
				obj.Position();
			}
		}
	}

	public bool isLoaded() {
		return mapTiles != null;
	}
	
	// Update is called once per frame
	void Update () {
        timePassed += Time.deltaTime;

        if(timePassed > TimeUnit)
        {
            timePassed = 0;

            foreach (var item in entities)
            {
                item.Update();
            }
        }
	}
}
