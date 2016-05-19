using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceTile : MapObject {

    int m_AvaliableResource = 0;

    char m_rawChar;

    uint TimeUnitsPassed = 0;

    public ResourceType m_resource;

	public void setTile(ResourceType Resource, IVec2 MapPos)
	{
		setTile (Resource, MapPos, 5);
	}

    public void setTile(ResourceType Resource, IVec2 MapPos, int amount)
    {
        m_MapPos = MapPos;
        m_resource = Resource;
        m_AvaliableResource = amount;
    }

    //should be called from the Map UpdateLoop every time unit
    public override void tick()
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
