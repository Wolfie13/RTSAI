using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceTile : MapObject {

    int m_AvaliableResource = 0;

    Building m_Building = null;

    char m_rawChar;

    uint TimeUnitsPassed = 0;

    public ResourceType m_resource;

    //list of people on the tile;
    List<Person> People = new List<Person>();


    public void setTile(ResourceType Resource, IVec2 MapPos, int amount = 5)
    {
        m_MapPos = MapPos;
        m_resource = Resource;
        m_AvaliableResource = amount;
    }

    //should be called from the Map UpdateLoop every time unit
    public override void Update()
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

   public int GatherResource(ResourceType Resource)
   {
       if (Resource == m_resource && m_AvaliableResource > 0)
       {
           m_AvaliableResource--;
           return 1;
       }
       return 0;
   }

}
