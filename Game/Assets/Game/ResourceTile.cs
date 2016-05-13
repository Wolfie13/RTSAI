using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceTile : MapObject {
    bool m_hasTree = false,
           m_hasOre = false,
           m_hasBuilding = false,
           m_isTraversable = false;

    int m_avaliableTimber = 0,
        m_avaliableStone = 5,
        m_avaliableIron = 5;

    Building m_Building = null;

    ivec2 m_MapPos;
    Vector3 m_realPos;

    char m_rawChar;

    uint TimeUnitsPassed = 0;

    [HideInInspector]
    public Dictionary<Resource, int> Resources = new Dictionary<Resource, int>();

    //list of people on the tile;
    List<Person> People = new List<Person>();


    public void setTile(char tile, ivec2 MapPos, Vector3 realPos)
    {
        m_rawChar = tile;
        m_MapPos = MapPos;
        m_realPos = realPos;

        if (Map.Trees.Contains(m_rawChar))
        {
            m_hasTree = true;
            m_isTraversable = true;
            m_avaliableTimber = 5;
        }
        if (Map.Terrain.Contains(m_rawChar))
        {
            m_isTraversable = true;
        }

        Resources[Resource.Timber] = 0;
        Resources[Resource.Ore] = 0;
        Resources[Resource.Stone] = 0;

    }

    //should be called from the Map UpdateLoop every time unit
    public void Update()
    {
        if (m_hasTree)
        {
            if (++TimeUnitsPassed > 10)
            {
                ++m_avaliableTimber;
                TimeUnitsPassed = 0;
            }
        }
    }

    //resource collection
    public int collectWood()
    {
        int temp = Resources[Resource.Timber];
        Resources[Resource.Timber] = 0;
        return temp;
    }

    public int collectStone()
    {
        int temp = Resources[Resource.Stone];
        Resources[Resource.Stone] = 0;
        return temp;
    }

    public int collectIron()
    {
        int temp = Resources[Resource.Ore];
        Resources[Resource.Ore] = 0;
        return temp;
    }

    public void cutTree()
    {
        if (m_avaliableTimber > 0)
        {
            --m_avaliableTimber;
            ++Resources[Resource.Timber];
        }
    }
    public void minestone()
    {
        if (m_avaliableStone > 0)
        {
            --m_avaliableStone;
            ++Resources[Resource.Stone];
        }
    }
    public void mineIron()
    {
        if (m_avaliableIron > 0)
        {
            --m_avaliableIron;
            ++Resources[Resource.Ore];
        }
    }


    //get and set

    public bool isTraversable() { return m_isTraversable; }
   

}
