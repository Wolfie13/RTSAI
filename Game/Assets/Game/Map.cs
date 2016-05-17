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
	private string MapName = "";
	private MapObject[,] entities;
	private char[,] mapTiles;
	public int sizeX;
	public int sizeY;
	public Material mapMaterial;

    public const int ChunkSize = 32;

    public GameObject Human = null;
    public int player1startX, player1startY,
               player2startX, player2startY;


    public GameObject BuildingTile = null;


    private List<Building> Buildings = new List<Building>();
    private List<Person> People = new List<Person>();

    private List<PlayerData> Players = new List<PlayerData>();

    public static Map CurrentMap = null;

    [Range(0,1)]
    public float ResourceChance = 0.5f;

    public static float TimeUnit = 1;

    private float timePassed = 0;

    public int NumberOfPlayers = 2;

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
    public void AddPerson(IVec2 Pos, int TeamID)
    {
        if(Human)
        {
            GameObject go = (GameObject)Instantiate(Human);
            go.GetComponent<Person>().teamID = TeamID;
            go.GetComponent<Person>().SetMapPosition(Pos);
            People.Add(go.GetComponent<Person>());
            Players[TeamID].People.Add(go.GetComponent<Person>());
        }
    }

    public void KillPerson(Person p)
    {
        GetTeamData(p.teamID).People.Remove(p);
        People.Remove(p);
        Destroy(p.gameObject);
    }

    public Person GetNonBusyPersonAt(IVec2 MapPos)
    {
        foreach (var item in GetPeopleAt(MapPos))
        {
            if (item.ToDoList.Count == 0 && item.BusyTime == 0)
                return item;
        }
        return null;
    }

    public List<Person> GetPeopleAtBuilding(Building b)
    {
        List<Person> result = new List<Person>();

        for (IVec2 offset = new IVec2(); offset.x < Building.Sizes[b.m_buildingtype].x; offset.x++)
        {
            for (offset.y = 0; offset.y < Building.Sizes[b.m_buildingtype].y; offset.y++)
            {
                result.AddRange(GetPeopleAt(b.m_MapPos + offset));
            }
        }

            return result;
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

    public bool BuildBuilding(BuildingType type, IVec2 Pos, int teamID)
    {
        bool isBuilding = false;

        if(CanBuild(type,Pos))
        {
            Building newBuilding = new Building();
            isBuilding = newBuilding.Build(type, Pos, teamID);
             if(isBuilding)
             {
                 for (IVec2 offset = new IVec2();  offset.x < Building.Sizes[type].x; offset.x++)
                 {
                     for (offset.y = 0; offset.y < Building.Sizes[type].y; offset.y++)
                     {
                         IVec2 newPos = Pos +offset;
                         entities[newPos.x, newPos.y] = newBuilding;
                         Instantiate(BuildingTile, getTilePos(newPos), Quaternion.Euler(Vector3.zero));
                     }
                 }

                  

             }
        }

        return isBuilding;
    }

    public bool CanBuild (BuildingType type, IVec2 Pos)
    {
        bool Buildable = true;

        for (IVec2 offset = new IVec2(); offset.x < Building.Sizes[type].x; offset.x++)
        {
            for (offset.y = 0; offset.y < Building.Sizes[type].y; offset.y++)
            {
                IVec2 newPos = Pos + offset;
                if (!Terrain.Contains(getTile(newPos)) || getObject(newPos) is Building)
                    Buildable = false;
            }
        }
        return Buildable;
    }

    //used only for testing 
    public void setResourcetile(ResourceType type, int ResourceAmount, IVec2 MapPos)
    {
        ResourceTile t = new ResourceTile();
        t.setTile(type, MapPos,ResourceAmount);
    }

    public PlayerData GetTeamData(int TeamID)
    {
        if (TeamID < 0 || TeamID >= Players.Count)
            return null;

        return Players[TeamID];
    }

	// Use this for initialization
    void Start()
    {

        if (CurrentMap != this)
        {
            Destroy(CurrentMap);
            CurrentMap = this;
        }

        load(MapName);
        initChunks();

        IVec2 Start1Pos = new IVec2(player1startX, player1startY);
        IVec2 Start2Pos = new IVec2(player2startX, player2startY);

        {/////////////////////player 1/////////////////////////
            PlayerData player = new PlayerData(Players.Count);
            Players.Add(player);
            AddPerson(Start1Pos, player.TeamID);
            for (Skill s = Skill.Labourer; s <= Skill.Rifleman; s++)
            {
                if (!player.People[0].Skills.Contains(s))
                    player.People[0].Skills.Add(s);
            }
            AddPerson(Start1Pos, player.TeamID);
            Players[Players.Count-1].Resources[ResourceType.Stone]++;
            Players[Players.Count - 1].Resources[ResourceType.Wood]++;
            BuildBuilding(BuildingType.Storage, Start1Pos, player.TeamID);
            
        }
        {///////////////////////////player2/////////////////////////
            PlayerData player = new PlayerData(Players.Count);
            Players.Add(player);
            AddPerson(Start2Pos, player.TeamID);
            for (Skill s = Skill.Labourer; s <= Skill.Rifleman; s++)
            {
                if (!player.People[0].Skills.Contains(s))
                    player.People[0].Skills.Add(s);
            }
            AddPerson(Start2Pos, player.TeamID);
            Players[Players.Count - 1].Resources[ResourceType.Stone]++;
            Players[Players.Count - 1].Resources[ResourceType.Wood]++;
            BuildBuilding(BuildingType.Storage, Start2Pos, player.TeamID);
        }

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
                if (Trees.Contains(mapTiles[i, lineCount]))
                {
                    entities[i, lineCount] = new ResourceTile();
                    ((ResourceTile)entities[i, lineCount]).setTile(ResourceType.Timber, new IVec2(i, lineCount));
                }
                else if (Terrain.Contains(mapTiles[i, lineCount]))
                {
                    if (UnityEngine.Random.Range(0, 1) < ResourceChance)
                    {
                        ResourceType t = (Mathf.FloorToInt(UnityEngine.Random.value) % 2 == 0) ? ResourceType.Iron : ResourceType.Coal;
                        entities[i, lineCount] = new ResourceTile();
                        ((ResourceTile)entities[i, lineCount]).setTile(t, new IVec2(i, lineCount));
                    }
                }
                else
                {
                    entities[i, lineCount] = new MapObject();
                }
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
                //Update Map item tick
                item.Update();
            }
        }
	}
}
