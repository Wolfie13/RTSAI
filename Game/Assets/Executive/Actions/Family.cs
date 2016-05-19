using System;
public class Family : Action
{

	public override ActionResult actionTick (Person person)
	{
		if (Map.CurrentMap.getObject(person.currentMapPos) is Building)
		{
			Building CurrentBuilding = (Building)Map.CurrentMap.getObject(person.currentMapPos);
			//check for another person 
			var other = CurrentBuilding.GetNonBusyPersonInBuilding();
			if (CurrentBuilding.teamID == person.teamID && other != null && other.teamID == person.teamID)
			{
				person.SetBusy(20);
				CurrentBuilding.GetNonBusyPersonInBuilding().SetBusy(20);
				if (CurrentBuilding.m_buildingtype == BuildingType.turfHut)
				{
					Map.CurrentMap.AddPerson(person.currentMapPos, person.teamID);
					return ActionResult.SUCCESS;
				}
				else if( CurrentBuilding.m_buildingtype == BuildingType.House)
				{
					Map.CurrentMap.AddPerson(person.currentMapPos, person.teamID);
					Map.CurrentMap.AddPerson(person.currentMapPos, person.teamID);
					return ActionResult.SUCCESS;
				}
			}
		}
		return ActionResult.FAIL;
	}
}

