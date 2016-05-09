using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapObject{

    bool m_hasTree = false, 
         m_hasOre = false,
         m_hasBuilding = false,
         m_isTraversable = false;

    int m_ammountTimber = 0,
        m_ammountStone = 0,
        m_ammountIron = 0,
        m_avaliableTimber = 0,
        m_avaliableStone = 5,
        m_avaliableIron = 5;

    Building m_Building = null;

    ivec2 m_MapPos;
    Vector3 m_realPos;

    char m_rawChar;

    uint TimeUnitsPassed = 0;

    //list of people on the tile;
    List<Person> People = new List<Person>();


    public void setTile(char tile, ivec2 MapPos, Vector3 realPos)
    {
        m_rawChar = tile;
        m_MapPos = MapPos;
        m_realPos = realPos;

        if(Map.Trees.Contains(m_rawChar))
        {
            m_hasTree = true;
            m_isTraversable = true;
            m_avaliableTimber = 5;
        }
        if(Map.Terrain.Contains(m_rawChar))
        {
            m_isTraversable = true;
        }
    }

    //should be called from the Map UpdateLoop every time unit
    public void Update()
    {
        if(m_hasTree)
        {
            if(++TimeUnitsPassed >10)
            {
                ++m_avaliableTimber;
                TimeUnitsPassed = 0;
            }
        }
    }

    //resource collection
    public int collectWood()
    {
        int temp = m_ammountTimber;
        m_ammountTimber = 0;
        return temp;
    }

    public int collectStone()
    {
        int temp = m_ammountStone;
        m_ammountStone = 0;
        return temp;
    }

    public int collectIron()
    {
        int temp = m_ammountIron;
        m_ammountIron = 0;
        return temp;
    }

    public void cutTree()
    {
        if (m_avaliableTimber > 0)
        {
            --m_avaliableTimber;
            ++m_ammountTimber;
        }
    }
    public void minestone()
    {
        if (m_avaliableStone > 0)
        {
            --m_avaliableStone;
            ++m_ammountStone;
        }
    }
    public void mineIron()
    {
        if (m_avaliableIron > 0)
        {
            --m_avaliableIron;
            ++m_ammountIron;
        }
    }


    //get and set

   public bool isTraversable() {return m_isTraversable;}
   public bool HasTrees() { return m_hasTree; }
   public bool HasOre() { return m_hasOre; }
   public Building GetBuilding() { return m_Building; }
   public void SetBuilding(Building b) { m_Building = b; }
   public void PersonEntered(Person p) { People.Add(p); }
   public List<Person> getPeople() { return People; }
   public void personLeft(Person p) { People.Remove(p); }

}
