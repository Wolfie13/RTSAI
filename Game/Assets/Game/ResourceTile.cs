using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceTile : MapObject {

    int m_AvaliableResource = 0;

    Building m_Building = null;

    ivec2 m_MapPos;
    Vector3 m_realPos;

    char m_rawChar;

    uint TimeUnitsPassed = 0;

    public ResourceType m_resource;

    //list of people on the tile;
    List<Person> People = new List<Person>();


    public void setTile(ResourceType Resource, ivec2 MapPos, Vector3 realPos)
    {
        m_MapPos = MapPos;
        m_realPos = realPos;

        m_resource = Resource;
    }

    //should be called from the Map UpdateLoop every time unit
    public void Update()
    {
        if (m_resource == ResourceType.Timber)
        {
            if (++TimeUnitsPassed > 10)
            {
                ++m_AvaliableResource;
                TimeUnitsPassed = 0;
            }
        }
    }

   public int collectResource(ResourceType Resource)
    {
        if (Resource != m_resource)
            return 0;

        int temp = m_AvaliableResource;
        m_AvaliableResource = 0;
        return temp;
    }

}
