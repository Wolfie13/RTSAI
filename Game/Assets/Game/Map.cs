using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public enum ResourceType
{
    Wood,
    Timber,
    Stone,
    Ore,
    Iron,
    Coal,
    Money,
    NumOfResourcetypes

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


    private List<Building> Buildings = new List<Building>();
    private List<Person> People = new List<Person>();

    public static Map CurrentMap = null;

    //resources
    public static Dictionary<ResourceType, int> GlobalResources = new Dictionary<ResourceType, int>();


    public static float TimeUnit = 1;

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

    public MapObject getObject(IVec2 pos) { return getObject(pos.x, pos.y); }

	public MapObject getObject (int x, int y)
	{
        if (x < 0 || x > sizeX || y < 0 || y > sizeY)
        {
            return null;
        }
		return entities [x, y];
	}

    public char getTile(IVec2 pos) { return getTile(pos.x, pos.y); }

	public char getTile(int x, int y)
	{
		if (x < 0 || x > sizeX || y < 0 || y > sizeY) {
			return '@';
		}
		return mapTiles [x, y];
	}

    public void setTile(IVec2 pos, char tile) { setTile(pos.x, pos.y, tile); }

	public void setTile(int x, int y, char tile)
	{
		if (x < 0 || x > sizeX || y < 0 || y > sizeY) {
			return;
		} else {
			mapTiles [x, y] = tile;
		}
	}

    public Vector3 getTilePos(IVec2 pos) { return getTilePos(pos.x, pos.y); }

    public Vector3 getTilePos(int x, int y)
    {
        float xPos = MapChunk.TILE_SIZE * x;
        float yPos = MapChunk.TILE_SIZE * y;

        return new Vector3(xPos, 0, yPos);
    }

    public IVec2 getTileFromPos(Vector3 worldPos)
    {
        //worldPos += new Vector3(0.5f, 0.5f, 0.5f) * MapChunk.TILE_SIZE;

        IVec2 mapPos = new IVec2(Mathf.FloorToInt(worldPos.x / MapChunk.TILE_SIZE), Mathf.FloorToInt(worldPos.z / MapChunk.TILE_SIZE));

        return mapPos;
    }

    public MapObject getobjectFromPos(Vector3 worldPos)
    {
        return getObject(getTileFromPos(worldPos));
    }


    //people
    public List<Person> GetPeopleWithSkill(Skill wantedSkill)
    {
        List<Person> results = new List<Person>();

        foreach (var item in People)
        {
            if (item.Skills.Contains(wantedSkill))
                results.Add(item);
        }
        return results;
    }
    public List<Person> GetPeople() { return People; }
    public List<Person> GetPeopleAt(IVec2 MapPos)
    {
        List<Person> results = new List<Person>();

        foreach (var item in People)
        {
            if (item.currentMapPos == MapPos)
                results.Add(item);
        }
        return results;
    }
    public void AddPerson(IVec2 Pos)
    {
        // code to create person here
    }

    //Buildings

    public List<Building> GetBuildingsOfType(BuildingType type)
    {
        List<Building> results = new List<Building>();

        foreach (var item in Buildings)
        {
            if (item.m_buildingtype == type)
                results.Add(item);
        }
        return results;
    }

    public List<Building> GetBuildings() { return Buildings; }

    public void BuildBuilding(BuildingType type, IVec2 Pos)
    {
        //code to build building
    }

    public bool CanBuild (BuildingType type, IVec2 Pos)
    {
        bool Buildable = true;

        for (IVec2 offset = new IVec2(); offset.x < Building.Sizes[type].x; offset.x++)
        {
            for (offset.y = 0; offset.y < Building.Sizes[type].y; offset.y++)
            {
                if(!Terrain.Contains(getTile(Pos + offset)))
                    Buildable = false;
            }
        }
        return Buildable;
    }

	// Use this for initialization
	void Start () {

        if (CurrentMap != this)
        {
            Destroy(CurrentMap);
            CurrentMap = this;
        }

		load (MapName);
		initChunks ();
        ResetResources();
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

    private void ResetResources()
    {
        GlobalResources.Clear();
        for (ResourceType i = ResourceType.Wood; i < ResourceType.NumOfResourcetypes; i++)
        {
            GlobalResources.Add(i, 0);
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
                //Update Map item tick
                //item.Update();
            }
        }
	}
}
