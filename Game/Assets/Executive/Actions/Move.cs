using System;

public class Move : Action
{
	int waitTicks = 0;

	public override ActionResult actionTick (Person person)
	{
		if (person.PathID > 0) {
			if (PathFinder.Paths [person.PathID].isPathFound) {
				if (PathFinder.Paths [person.PathID].FoundPath.Count > 0) {
					person.currentMapPos = PathFinder.Paths [person.PathID].FoundPath [0].MapPos;
					person.transform.position = Map.getTilePos (person.currentMapPos);
				
					PathFinder.Paths [person.PathID].FoundPath.RemoveAt (0);
					return ActionResult.CONTINUE;
				} else {
					return ActionResult.SUCCESS;
				}
				
			} else {
				//Give the pathfinder 5 ticks to figure it out.
				if (waitTicks++ > 4) {
					return ActionResult.FAIL;
				}
				return ActionResult.CONTINUE;
			}
		} else {
			return ActionResult.FAIL;
		}
	}
}

