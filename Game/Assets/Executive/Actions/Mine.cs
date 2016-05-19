using System;
using UnityEngine;

public class Mine : Action
{
	public Mine()
	{
		type = ResourceType.Ore;
	}

	public Mine(ResourceType rt)
	{
		type = rt;
	}

	private ResourceType type;
	private IVec2 destination = null;

	public override ActionResult actionTick (Person person)
	{
		if (destination == null) {
			ResourceTile tile = Map.CurrentMap.GetNearestResourceTile (person.currentMapPos, type);
			Building nearestMine = Map.CurrentMap.GetNearestBuilding (person.currentMapPos, BuildingType.Mine);

			float distanceToTile = (tile.m_MapPos - person.currentMapPos).magnitude ();
			float distanceToBuilding = nearestMine != null ? (nearestMine.m_MapPos - person.currentMapPos).magnitude () : float.MaxValue;

			if (distanceToTile > distanceToBuilding) {
				destination = nearestMine.m_MapPos;
			} else {
				destination = tile.m_MapPos;
			}
		}

		if (person.currentMapPos == destination) {
			MapObject mo = Map.CurrentMap.getObject(destination);
			if (mo is Building) {
				Building b = mo as Building;
				if (b.m_buildingtype == BuildingType.Mine && person.Skills.Contains(Skill.Miner))
				{
					person.Resources[type] += 1;
					person.ToDoList[0] = new Store();
					return ActionResult.CONTINUE;
				} else {
					return ActionResult.FAIL;
				}
			}
			if (mo is ResourceTile) {
				ResourceTile rt = mo as ResourceTile;
				if (rt.m_resource == type) {
					person.Resources[rt.m_resource] += rt.GatherResource(type);
					person.SetBusy(5);
					person.ToDoList[0] = new Store();
					return ActionResult.CONTINUE;
				} else {
					return ActionResult.FAIL;
				}
			}
		}

		person.ToDoList.Insert (0, new Move (person.currentMapPos, destination));
		return ActionResult.CONTINUE;
	}
}

