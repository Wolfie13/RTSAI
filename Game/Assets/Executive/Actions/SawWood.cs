using System;
public class SawWood : Action
{

	public override ActionResult actionTick (Person person)
	{
		var CurrentTeamData = Map.CurrentMap.GetTeamData (person.teamID);
		if (Map.CurrentMap.getObject(person.currentMapPos) is Building)
		{
			Building CurrentBuilding = (Building)Map.CurrentMap.getObject(person.currentMapPos);
			
			if (CurrentBuilding.m_buildingtype == BuildingType.Sawmill
			    && CurrentBuilding.teamID == person.teamID
			    && person.Skills.Contains(Skill.Labourer)
			    && CurrentTeamData.Resources[ResourceType.Timber] > 0)
			{
				person.SetBusy(10);
				CurrentTeamData.Resources[ResourceType.Timber]--;
				person.Resources[ResourceType.Wood]++;
			}
		}
		return ActionResult.FAIL;
	}
}

