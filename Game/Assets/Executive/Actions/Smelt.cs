using System;
public class Smelt : Action
{

	public override ActionResult actionTick (Person person)
	{
		var CurrentTeamData = Map.CurrentMap.GetTeamData (person.teamID);
		if (Map.CurrentMap.getObject(person.currentMapPos) is Building)
		{
			Building CurrentBuilding = (Building)Map.CurrentMap.getObject(person.currentMapPos);
			
			if (CurrentBuilding.m_buildingtype == BuildingType.Smelter
			    && CurrentBuilding.teamID == person.teamID
			    && person.Skills.Contains(Skill.Labourer)
			    && CurrentTeamData.Resources[ResourceType.Ore] > 0
			    && CurrentTeamData.Resources[ResourceType.Coal] > 0)
			{
				person.SetBusy(5);
				CurrentTeamData.Resources[ResourceType.Ore]--;
				CurrentTeamData.Resources[ResourceType.Coal]--;
				
				person.Resources[ResourceType.Iron]++;
			}
		}
		return ActionResult.FAIL;
	}
}

