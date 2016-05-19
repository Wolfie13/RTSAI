using System;
using UnityEngine;

public class Mine : Action
{

	public override ActionResult actionTick (Person person)
	{
		if (Map.CurrentMap.getObject(person.currentMapPos) is Building)
		{
			Building CurrentBuilding = (Building)Map.CurrentMap.getObject(person.currentMapPos);
			
			if (person.Skills.Contains(Skill.Miner)
			    && CurrentBuilding.m_buildingtype == BuildingType.Mine
			    && CurrentBuilding.teamID == person.teamID)
			{
				person.SetBusy(5);   
				//random chance to get coal or ore
				ResourceType r = ((Mathf.FloorToInt(UnityEngine.Random.value) % 2) == 0) ? ResourceType.Coal : ResourceType.Ore;
				person.Resources[r]++;
				return ActionResult.SUCCESS;
			}
		}
		else if(Map.CurrentMap.getObject(person.currentMapPos) is ResourceTile)
		{
			ResourceTile tile = (ResourceTile)Map.CurrentMap.getObject(person.currentMapPos);
			if (tile.m_resource == ResourceType.Ore
			    && person.Skills.Contains(Skill.Miner))
			{
				person.Resources[ResourceType.Ore] += tile.GatherResource(ResourceType.Ore);
				person.SetBusy(5);
				return ActionResult.SUCCESS;
			}
			else if (tile.m_resource == ResourceType.Coal
			         && person.Skills.Contains(Skill.Miner))
			{
				person.Resources[ResourceType.Coal] += tile.GatherResource(ResourceType.Coal);
				person.SetBusy(5);
				return ActionResult.SUCCESS;
			}
		}
		return ActionResult.FAIL;
	}
}

