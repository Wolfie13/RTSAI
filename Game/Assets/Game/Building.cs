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



public class Building : MapObject {

    public static Dictionary<BuildingType, IVec2> Sizes = new Dictionary<BuildingType, IVec2>()
    {
        {BuildingType.none,         new IVec2(1,1)},
        {BuildingType.turfHut,      new IVec2(2,2)},
        {BuildingType.House,        new IVec2(2,3)},
        {BuildingType.School,       new IVec2(5,5)},
        {BuildingType.Barracks,     new IVec2(5,5)},
        {BuildingType.Storage,      new IVec2(5,5)},
        {BuildingType.Mine,         new IVec2(1,1)},
        {BuildingType.Smelter,      new IVec2(2,2)},
        {BuildingType.Quarry,       new IVec2(1,1)},
        {BuildingType.Sawmill,      new IVec2(3,3)},//may change
        {BuildingType.Blacksmith,   new IVec2(3,3)},//maychange
        {BuildingType.MarketStall,  new IVec2(1,1)}

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
    [HideInInspector]
    public List<Person> people = new List<Person>();
    [HideInInspector]
    public Dictionary<Resources, int> Resources = new Dictionary<Resources, int>();

    public BuildingType m_buildingtype;

    IVec2 m_buildingSize;

    [HideInInspector]
    public IVec2 m_MapPos = new IVec2();

    //returns true if sucsessful
    public bool Build(BuildingType buildingType, IVec2 mapPos)
    {

        // create build here
        return true;
    }
}
