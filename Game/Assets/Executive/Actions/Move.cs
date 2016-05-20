using System;

public class Move : Action
{
	//int waitTicks = 0;
	public path path;

	public Move(IVec2 source, IVec2 destination)
	{
		path = (new AStar ()).GetPath (source, destination);
	}

	public override ActionResult actionTick (Person person)
	{
		person.currentMapPos = path.FoundPath [0].MapPos;
		person.transform.position = Map.getTileCenterPos (person.currentMapPos);
		
		path.FoundPath.RemoveAt (0);

		if (path.FoundPath.Count <= 0) {
			return ActionResult.SUCCESS;
		} else {
			return ActionResult.CONTINUE;
		}
		//return ActionResult.FAIL;
	}
}

