using UnityEngine;
using System.Collections;


public enum Building
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

public class MapObject{

    bool m_hasTree = false, 
         m_hasOre = false,
         m_hasBuilding = false,
         m_isTraversable = false;

    int m_ammountWood = 0,
        m_ammountStone = 0,
        m_ammountIron = 0,
        m_avaliableWood = 0,
        m_avaliableStone = 5,
        m_avaliableIron = 5;

    Building m_Building = Building.none;

    ivec2 m_MapPos;
    Vector3 m_realPos;

    char m_rawChar;

    uint TimeUnitsPassed = 0;

    public void setTile(char tile, ivec2 MapPos, Vector3 realPos)
    {
        m_rawChar = tile;
        m_MapPos = MapPos;
        m_realPos = realPos;

        if(Map.Trees.Contains(m_rawChar))
        {
            m_hasTree = true;
            m_isTraversable = true;
            m_avaliableWood = 5;
        }
        if(Map.Terrain.Contains(m_rawChar))
        {
            m_isTraversable = true;
        }
    }

    //should be called from the Map UpdateLoop every time unit
    void Update()
    {
        switch(m_Building)
        {
            default:
                break;
        }

        if(m_hasTree)
        {
            if(++TimeUnitsPassed >10)
            {
                ++m_avaliableWood;
                TimeUnitsPassed = 0;
            }
        }
    }

    //get and set

   public bool isTraversable() {return m_isTraversable;}
   public bool HasTrees() { return m_hasTree; }
   public bool HasOre() { return m_hasOre; }
   public Building GetBuilding() { return m_Building; }
   public void SetBuilding(Building b) { m_Building = b; }
   

}
