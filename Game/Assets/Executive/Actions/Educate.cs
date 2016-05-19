using System;
public class Educate : Action
{

	public override ActionResult actionTick (Person person)
	{
		if (Map.CurrentMap.getObject(person.currentMapPos) is Building)
		{
			Building CurrentBuilding = (Building)Map.CurrentMap.getObject(person.currentMapPos);
			
			var other = CurrentBuilding.GetNonBusyPersonInBuilding();
			
			if (person.Skills.Contains(Skill.Rifleman)
			    && CurrentBuilding.m_buildingtype == BuildingType.Barracks
			    && CurrentBuilding.teamID == person.teamID
			    && other != null
			    && other.teamID == person.teamID)
			{
				
				person.SetBusy(30);
				other.SetBusy(30);
				other.Skills.Add(Skill.Rifleman);
			}
		}
		else
		{
			Person other = Map.CurrentMap.GetNonBusyPersonAt(person.currentMapPos);
			if (other == null || other.teamID != person.teamID)
				return ActionResult.FAIL;
			
			foreach (var item in person.Skills)
			{
				if(!other.Skills.Contains(item))
				{
					other.SetBusy(100);
					person.SetBusy(100);
					other.Skills.Add(item);
					return ActionResult.SUCCESS;
				}
			}
		}
		return ActionResult.FAIL;
	}
}

