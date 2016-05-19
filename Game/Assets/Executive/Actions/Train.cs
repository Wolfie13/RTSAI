using System;
public class Train : Action
{

	public override ActionResult actionTick (Person person)
	{
		if (Map.CurrentMap.getObject(person.currentMapPos) is Building)
		{
			Building CurrentBuilding = (Building)Map.CurrentMap.getObject(person.currentMapPos);
			
			if (CurrentBuilding.m_buildingtype == BuildingType.School
			    && CurrentBuilding.teamID == person.teamID)
			{
				var other = CurrentBuilding.GetNonBusyPersonInBuilding();
				while (other != null && other.teamID == person.teamID)
				{
					foreach (var item in person.Skills)
					{
						if (!other.Skills.Contains(item))
						{
							other.SetBusy(50);
							person.SetBusy(50);
							other.Skills.Add(item);
							return ActionResult.SUCCESS;
						}
					}
					other = CurrentBuilding.GetNonBusyPersonInBuilding();
				}
			}
			
		}
		return ActionResult.FAIL;
	}
}

