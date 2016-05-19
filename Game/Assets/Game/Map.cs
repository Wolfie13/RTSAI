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
    Rifles,
	Iron,
	Coal,
	Money,
	NumOfResourcetypes

}

public class Map : MonoBehaviour
{
	[SerializeField]
	private string
		MapName = "";
	private MapObject[,] entities;
	private char[,] mapTiles;
	public int sizeX;
	public int sizeY;
	public Material mapMaterial;
	public const int ChunkSize = 32;
	public GameObject Human = null;
	public IVec2 player1Start, player2Start;
	public GameObject BuildingTile = null, CoalTile = null, OreTile = null;
	private List<Building> Buildings = new List<Building> ();
	private List<Person> People = new List<Person> ();
	private List<PlayerData> Players = new List<PlayerData> ();
	public static Map CurrentMap = null;
	[Range(0,1)]
	public float
		ResourceChance = 0.1f;
	public static float TimeUnit = 1;
	private float timePassed = 0;
	public int NumberOfPlayers = 2;

	//passable
	public static IList<char> Terrain = new List<char>{ '.', 'G' }.AsReadOnly ();
	//out of bounds
	public static IList<char> OutBounds = new List<char> { '@', 'O' }.AsReadOnly ();
	//Unpassable
	public static IList<char> Trees = new List<char> { 'T' }.AsReadOnly ();
	//passable from regular terrain
	public static IList<char> swamp = new List<char> { 'S' }.AsReadOnly ();
	//traversable, but not passable from terrain
	public static IList<char> water = new List<char> { 'W' }.AsReadOnly ();

	public MapObject getObject (IVec2 pos)
	{
		return getObject (pos.x, pos.y);
	}

	public MapObject getObject (int x, int y)
	{
		if (x < 0 || x > sizeX || y < 0 || y > sizeY) {
			return null;
		}
		return entities [x, y];
	}

	public char getTile (IVec2 pos)
	{
		return getTile (pos.x, pos.y);
	}

	public char getTile (int x, int y)
	{
		if (x < 0 || x > sizeX || y < 0 || y > sizeY) {
			return '@';
		}
		return mapTiles [x, y];
	}

	public void setTile (IVec2 pos, char tile)
	{
		setTile (pos.x, pos.y, tile);
	}

	public void setTile (int x, int y, char tile)
	{
		if (x < 0 || x > sizeX || y < 0 || y > sizeY) {
			return;
		} else {
			mapTiles [x, y] = tile;
		}
	}

	public static Vector3 getTilePos (IVec2 pos)
	{
		return getTilePos (pos.x, pos.y);
	}

	public static Vector3 getTilePos (int x, int y)
	{
		float xPos = MapChunk.TILE_SIZE * x;
		float yPos = MapChunk.TILE_SIZE * y;

		return new Vector3 (xPos, 0, yPos);
	}

	public static Vector3 getTileCenterPos (IVec2 pos)
	{
		return getTileCenterPos (pos.x, pos.y);
	}

	public static Vector3 getTileCenterPos (int x, int y)
	{
		float xPos = MapChunk.TILE_SIZE * x;
		float yPos = MapChunk.TILE_SIZE * y;
		
		return new Vector3 (xPos + MapChunk.TILE_SIZE / 2, 0, yPos + MapChunk.TILE_SIZE / 2);
	}

	public static IVec2 getTileFromPos (Vector3 worldPos)
	{
		//worldPos += new Vector3(0.5f, 0.5f, 0.5f) * MapChunk.TILE_SIZE;

		IVec2 mapPos = new IVec2 (Mathf.FloorToInt (worldPos.x / MapChunk.TILE_SIZE), Mathf.FloorToInt (worldPos.z / MapChunk.TILE_SIZE));

		return mapPos;
	}

	public MapObject getobjectFromPos (Vector3 worldPos)
	{
		return getObject (getTileFromPos (worldPos));
	}


	//people
	public List<Person> GetPeopleWithSkill (Skill wantedSkill)
	{
		List<Person> results = new List<Person> ();

		foreach (var item in People) {
			if (item.Skills.Contains (wantedSkill))
				results.Add (item);
		}
		return results;
	}

	public List<Person> GetPeople ()
	{
		return People;
	}

	public List<Person> GetPeopleAt (IVec2 MapPos)
	{
		List<Person> results = new List<Person> ();

		foreach (var item in People) {
			if (item.currentMapPos == MapPos)
				results.Add (item);
		}
		return results;
	}

	public void AddPerson (IVec2 Pos, int TeamID)
	{
		if (Human) {
			GameObject go = (GameObject)Instantiate (Human);
			go.renderer.material = GameObject.FindObjectOfType<MaterialSource>().getMaterialByName("Team" + (TeamID + 1).ToString());
			go.name = "Person" + TeamID + Players[TeamID].People.Count;
			go.GetComponent<Person> ().teamID = TeamID;
			go.GetComponent<Person> ().SetMapPosition (Pos);
			People.Add (go.GetComponent<Person> ());
			Person p = go.GetComponent<Person> ();
			p.ToDoList.Add(new CutTree());
			Players [TeamID].People.Add (p);
			go.transform.parent = this.transform;
		}
	}

	public void KillPerson (Person p)
	{
		GetTeamData (p.teamID).People.Remove (p);
		People.Remove (p);
		Destroy (p.gameObject);
	}

	public Person GetNonBusyPersonAt (IVec2 MapPos)
	{
		foreach (var item in GetPeopleAt(MapPos)) {
			if (item.ToDoList.Count == 0 && item.BusyTime == 0)
				return item;
		}
		return null;
	}

	public List<Person> GetPeopleAtBuilding (Building b)
	{
		List<Person> result = new List<Person> ();

		IVec2 offset = new IVec2();
		IVec2 size = Building.Sizes[b.m_buildingtype];
		for (offset.x = -size.x / 2; offset.x < size.x / 2; offset.x++) {
			for (offset.y = -size.y / 2; offset.y < size.y / 2; offset.y++) {
				result.AddRange (GetPeopleAt (b.m_MapPos + offset));
			}
		}

		return result;
	}

	//Buildings

	public List<Building> GetBuildingsOfType (BuildingType type)
	{
		List<Building> results = new List<Building> ();

		foreach (var item in Buildings) {
			if (item.m_buildingtype == type)
				results.Add (item);
		}
		return results;
	}

	public List<Building> GetBuildings ()
	{
		return Buildings;
	}

	public bool BuildBuilding (BuildingType type, IVec2 Pos, int teamID)
	{
		return BuildBuilding (type, Pos, teamID, false);
	}


	public bool BuildBuilding (BuildingType type, IVec2 Pos, int teamID, bool force)
	{
		bool isBuilding = false;

		if (CanBuild (type, Pos) || force) {
			Building newBuilding = new Building ();
			isBuilding = newBuilding.Build (type, Pos, teamID);
			if (isBuilding || force) {
				IVec2 offset = new IVec2();
				IVec2 size = Building.Sizes[type];
				for (offset.x = -size.x / 2; offset.x < size.x / 2; offset.x++) {
					for (offset.y = -size.y / 2; offset.y < size.y / 2; offset.y++) {
						IVec2 newPos = Pos + offset;
						entities [newPos.x, newPos.y] = newBuilding;
					}
				}

				GameObject obj = Instantiate (BuildingTile, Vector3.zero, Quaternion.Euler (Vector3.zero)) as GameObject;
				obj.renderer.material = GameObject.FindObjectOfType<MaterialSource>().getMaterialByName("Team" + (teamID + 1).ToString());
				obj.transform.SetParent(chunks[Pos.x / 32, Pos.y / 32].transform);
				obj.transform.position = getTileCenterPos (Pos);
				Vector3 oldScale = obj.transform.localScale;
				obj.transform.localScale = new Vector3(size.x * oldScale.x, 1 * oldScale.y, size.y * oldScale.z);
				obj.name = "Building" + Buildings.Count + "(" + type.ToString() + ")";
				newBuilding.gameObject = obj;
				Buildings.Add(newBuilding);
				Players[teamID].Buildings.Add(newBuilding);
			}
		}

		return isBuilding;
	}

	public bool CanBuild (BuildingType type, IVec2 Pos)
	{
		bool Buildable = true;

		IVec2 offset = new IVec2();
		IVec2 size = Building.Sizes[type];
		for (offset.x = -size.x / 2; offset.x < size.x / 2; offset.x++) {
			for (offset.y = -size.y / 2; offset.y < size.y / 2; offset.y++) {
				IVec2 newPos = Pos + offset;
				if (!Terrain.Contains (getTile (newPos)) || getObject (newPos) is Building) {
					Buildable = false;
				} else if (type == BuildingType.Mine) {
					ResourceTile t = getObject (newPos) as ResourceTile;
					if (t == null || (t.m_resource != ResourceType.Coal 
						&& t.m_resource != ResourceType.Ore)) {
						Buildable = false;
					}
				}

			}
		}
		return Buildable;
	}

	//used only for testing 
	public void setResourcetile (ResourceType type, int ResourceAmount, IVec2 MapPos)
	{
		ResourceTile t = new ResourceTile ();
		t.setTile (type, MapPos, ResourceAmount);
	}
	//
	public ResourceTile GetNearestResourceTile (IVec2 Pos, ResourceType type)
	{
		ResourceTile nearestTile = null;
		int nearestDistance = int.MaxValue;
		foreach (MapObject mo in entities) {
			if (mo is ResourceTile) {
				ResourceTile rt = mo as ResourceTile;
				if (rt.m_resource == type) {
					int distance = Pos.manhatttanDistance(rt.m_MapPos);
					if (distance < nearestDistance){
						nearestDistance = distance;
						nearestTile = rt;
					}
				}
				
			}
		}
		return nearestTile;
	}

	public Building GetNearestBuilding (IVec2 Pos, BuildingType type)
	{
		Building nearestBuilding = null;
		int nearestDistance = int.MaxValue;
		foreach (MapObject mo in entities) {
			if (mo is Building) {
				Building b = mo as Building;
				if (b.m_buildingtype == type) {
					int distance = Pos.manhatttanDistance(b.m_MapPos);
					if (distance < nearestDistance){
						nearestDistance = distance;
						nearestBuilding = b;
					}
				}
				
			}
		}
		return nearestBuilding;
	}
	
	public PlayerData GetTeamData (int TeamID)
	{
		if (TeamID < 0 || TeamID >= Players.Count)
			return null;

		return Players [TeamID];
	}

	// Use this for initialization
	void Awake ()
	{

		if (CurrentMap != this) {
			Destroy (CurrentMap);
			CurrentMap = this;
		}

		load (MapName);
		initChunks ();

		{/////////////////////player 1/////////////////////////
			PlayerData player = new PlayerData (Players.Count);
			Players.Add (player);
			AddPerson (player1Start + new IVec2(0, -10), player.TeamID);
			for (Skill s = Skill.Labourer; s <= Skill.Rifleman; s++) {
				if (!player.People [0].Skills.Contains (s))
					player.People [0].Skills.Add (s);
			}
			//AddPerson (player1Start + new IVec2(0, 10), player.TeamID);
			player.Resources [ResourceType.Stone]++;
			player.Resources [ResourceType.Wood]++;
			BuildBuilding (BuildingType.Storage, player1Start, player.TeamID, true);
		}
		{///////////////////////////player2/////////////////////////
			PlayerData player = new PlayerData (Players.Count);
			Players.Add (player);
			//AddPerson (player2Start + new IVec2(0, -10), player.TeamID);
			//for (Skill s = Skill.Labourer; s <= Skill.Rifleman; s++) {
				//if (!player.People [0].Skills.Contains (s))
					//player.People [0].Skills.Add (s);
			//}
			//AddPerson (player2Start + new IVec2(0, 10), player.TeamID);
			player.Resources [ResourceType.Stone]++;
			player.Resources [ResourceType.Wood]++;
			BuildBuilding (BuildingType.Storage, player2Start, player.TeamID, true);
		}

		foreach (var item in People) {
			item.SetBusy (0);
		}

		//Position the camera
		Camera.main.transform.position = getTilePos (player1Start) + new Vector3 (0, 100, 0);
	}

	void load (string filename)
	{
		// Read the file
		System.IO.StreamReader file = 
			new System.IO.StreamReader (filename);

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
		while ((line = file.ReadLine()) != null) {
			for (int i = 0; i < line.Length; i++) {
				mapTiles [i, lineCount] = line [i];
				if (Trees.Contains (mapTiles [i, lineCount])) {
					entities [i, lineCount] = new ResourceTile ();
					((ResourceTile)entities [i, lineCount]).setTile (ResourceType.Timber, new IVec2 (i, lineCount));
				} else if (Terrain.Contains (mapTiles [i, lineCount])) {
					if (UnityEngine.Random.Range (0.0f, 1.0f) < ResourceChance) {
						ResourceType t = ((UnityEngine.Random.Range (1, 100) % 2) == 0) ? ResourceType.Iron : ResourceType.Coal;
						ResourceTile rt = new ResourceTile ();
						entities [i, lineCount] = rt;
						rt.setTile (t, new IVec2 (i, lineCount));
					}
				} else {
					entities [i, lineCount] = new MapObject ();
				}
			}

			lineCount++;
		}

		//close file
		file.Close ();

	}



	private MapChunk[,] chunks;
	public void initChunks ()
	{
		int chunksX = this.sizeX / ChunkSize;
		int chunksY = this.sizeY / ChunkSize;
		chunks = new MapChunk[chunksX, chunksY];
		for (int i = 0; i != chunksX; i++) {
			for (int j = 0; j != chunksY; j++) {
				GameObject newChunk = new GameObject ("Chunk" + i + "," + j);
				newChunk.transform.parent = this.gameObject.transform;
				MapChunk obj = newChunk.AddComponent<MapChunk> ();
				obj.Generate (i, j, this);
				chunks[i, j] = obj;
			}
		}

		foreach (MapObject mo in entities) {
			if (mo is ResourceTile) {
				ResourceTile rt = mo as ResourceTile;
				if (rt.m_resource == ResourceType.Timber) {
					continue;
				}
				GameObject newTile = null;
				if (rt.m_resource == ResourceType.Coal) {
					newTile = Instantiate (CoalTile, getTileCenterPos (rt.m_MapPos), Quaternion.Euler (Vector3.zero)) as GameObject;
				} else if (rt.m_resource == ResourceType.Iron) {
					newTile = Instantiate (OreTile, getTileCenterPos (rt.m_MapPos), Quaternion.Euler (Vector3.zero)) as GameObject;
				}
				if (newTile != null) {
					newTile.transform.SetParent(chunks[rt.m_MapPos.x / 32, rt.m_MapPos.y / 32].transform);
					mo.gameObject = newTile;
				}
			}

		}
	}
	
	public bool isLoaded ()
	{
		return mapTiles != null;
	}
	
	// Update is called once per frame
	void Update ()
	{
		timePassed += Time.deltaTime;

		if (timePassed > TimeUnit) {
			timePassed = 0;

			foreach (Building b in Buildings) {
				b.tick ();
			}

			foreach (Person p in People) {
				p.personTick();
			}

			GameObject.FindObjectOfType<Executive>().tick();
		}
	}
}
