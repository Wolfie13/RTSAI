using System;

public abstract class Action
{
	public enum ActionResult {
		FAIL,
		SUCCESS,
		CONTINUE
	}

	public abstract ActionResult actionTick(Person person);
}

