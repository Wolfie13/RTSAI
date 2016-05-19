using System;

public class BuildBuilding : Action
{
	public BuildBuilding(BuildingType bt)
	{
		this.type = bt;
	}

	BuildingType type;
	IVec2 buildPos = null;

	public override ActionResult actionTick (Person person)
	{
		if (buildPos == null) {
			buildPos = Map.CurrentMap.FindLocationToBuild (person.currentMapPos, type);
		}

		if (buildPos != null) {
			if (person.currentMapPos == buildPos) {
				bool success = Map.CurrentMap.BuildBuilding(type, buildPos, person.teamID);
				if (success) {
					return ActionResult.SUCCESS;
				} else {
					return ActionResult.FAIL;
				}
			}

			person.ToDoList.Insert (0, new Move (person.currentMapPos, buildPos));
			return ActionResult.CONTINUE;
		}
		return ActionResult.FAIL;
	}
}