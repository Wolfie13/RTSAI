using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BuildingType
{
    none,
    turfHut,
    House,
    School,
    Barracks,
    Storage,
    Mine,
    Smelter,
    Quarry,
    Sawmill,
    Blacksmith,
    MarketStall
}



public class Building : MonoBehaviour {

    public static Dictionary<BuildingType, ivec2> Sizes = new Dictionary<BuildingType, ivec2>()
    {
        {BuildingType.none,         new ivec2(1,1)},
        {BuildingType.turfHut,      new ivec2(2,2)},
        {BuildingType.House,        new ivec2(2,3)},
        {BuildingType.School,       new ivec2(5,5)},
        {BuildingType.Barracks,     new ivec2(5,5)},
        {BuildingType.Storage,      new ivec2(5,5)},
        {BuildingType.Mine,         new ivec2(1,1)},
        {BuildingType.Smelter,      new ivec2(2,2)},
        {BuildingType.Quarry,       new ivec2(1,1)},
        {BuildingType.Sawmill,      new ivec2(3,3)},//may change
        {BuildingType.Blacksmith,   new ivec2(3,3)},//maychange
        {BuildingType.MarketStall,  new ivec2(1,1)}

    };


    public static Dictionary<BuildingType, int> BuildTime = new Dictionary<BuildingType, int>()
    {
        {BuildingType.none,         0},
        {BuildingType.turfHut,     10},
        {BuildingType.House,       15},
        {BuildingType.School,      30},
        {BuildingType.Barracks,    30},
        {BuildingType.Storage,     20},
        {BuildingType.Mine,        50},
        {BuildingType.Smelter,     20},
        {BuildingType.Quarry,      10},
        {BuildingType.Sawmill,     30},
        {BuildingType.Blacksmith,  30},
        {BuildingType.MarketStall,  5}

    };

    //lk=ist of tiles coved by th building
    List<MapObject> Tiles = new List<MapObject>();
    //list of people at the building
    List<Person> people = new List<Person>();

    BuildingType m_buildingtype;

    ivec2 m_buildingSize;

    //returns true if sucsessful
    public bool Build(BuildingType buildingType, ivec2 mapPos)
    {
        return true;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
