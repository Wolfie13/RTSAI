using System;
using System.Collections.Generic;

public class Learn : Action
{
	public Learn (IVec2 mutualLearningZone)
	{
		dest = mutualLearningZone;
	}

	private IVec2 dest;

	public override ActionResult actionTick (Person person)
	{
		if (person.currentMapPos == dest) {
			return ActionResult.SUCCESS;
		} else {
			//go to the lesson
			person.ToDoList.Insert (0, new Move (person.currentMapPos, dest));
			return ActionResult.CONTINUE;
		}
		return ActionResult.CONTINUE;
	}
}
