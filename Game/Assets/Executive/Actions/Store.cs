using System;
public class Store : Action
{
	
	public override ActionResult actionTick (Person person)
	{
		if (Map.CurrentMap.getObject(person.currentMapPos) is Building)
		{
			Building CurrentBuilding = (Building)Map.CurrentMap.getObject(person.currentMapPos);
			
			if (CurrentBuilding.m_buildingtype == BuildingType.Storage
			    && CurrentBuilding.teamID == person.teamID)
			{
				person.SetBusy(1);
				foreach (var item in person.Resources)
				{
					Map.CurrentMap.GetTeamData(person.teamID).Resources[item.Key] += item.Value;
					person.Resources[item.Key] = 0;
				}
			}
		}
		return ActionResult.FAIL;
	}
	
}