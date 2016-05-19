using System;
public class Smelt : Action
{
	private IVec2 destination = null;
	public override ActionResult actionTick (Person person)
	{
		PlayerData team = Map.CurrentMap.GetTeamData (person.teamID);
		if (destination == null) {
			Building nearestMine = team.GetNearestBuilding (person.currentMapPos, BuildingType.Smelter);
			if (nearestMine == null) {
				return ActionResult.FAIL;
			}
			destination = nearestMine.m_MapPos;
		}
		
		if (person.currentMapPos == destination) {
			MapObject mo = Map.CurrentMap.getObject(destination);
			if (mo is Building) {
				Building b = mo as Building;
				if (b.m_buildingtype == BuildingType.Smelter && team.Resources[ResourceType.Ore] > 0 && team.Resources[ResourceType.Coal] > 0)
				{
					team.Resources[ResourceType.Ore]--;
					team.Resources[ResourceType.Coal]--;
					person.Resources[ResourceType.Iron] += 1;
					person.ToDoList[0] = new Store();
					return ActionResult.CONTINUE;
				}
			}
			return ActionResult.FAIL;
		}
		
		person.ToDoList.Insert (0, new Move (person.currentMapPos, destination));
		return ActionResult.CONTINUE;
	}
}

