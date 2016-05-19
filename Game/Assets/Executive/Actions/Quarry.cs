using System;
public class Quarry : Action
{
	private IVec2 destination = null;
	
	public override ActionResult actionTick (Person person)
	{
		if (destination == null) {
			Building nearestMine = Map.CurrentMap.GetNearestBuilding (person.currentMapPos, BuildingType.Quarry);
			if (nearestMine == null) {
				return ActionResult.FAIL;
			}
			destination = nearestMine.m_MapPos;
		}
		
		if (person.currentMapPos == destination) {
			MapObject mo = Map.CurrentMap.getObject(destination);
			if (mo is Building) {
				Building b = mo as Building;
				if (b.m_buildingtype == BuildingType.Quarry)
				{
					person.Resources[ResourceType.Stone] += 1;
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

