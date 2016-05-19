using System;
public class Quarry : Action
{

	public override ActionResult actionTick (Person person)
	{
		if (Map.CurrentMap.getObject(person.currentMapPos) is Building)
		{
			Building CurrentBuilding = (Building)Map.CurrentMap.getObject(person.currentMapPos);
			
			if (CurrentBuilding.m_buildingtype == BuildingType.Quarry
			    && CurrentBuilding.teamID == person.teamID
			    && person.Skills.Contains(Skill.Labourer))
			{
				person.SetBusy(5);
				person.Resources[ResourceType.Stone]++;
				return ActionResult.SUCCESS;
			}
		}
		return ActionResult.FAIL;
	}
}

