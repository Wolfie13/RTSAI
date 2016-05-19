using System;
public class CutTree : Action
{

	public override ActionResult actionTick (Person person)
	{
		if (!person.Skills.Contains (Skill.Lumberjack)) {
			return ActionResult.FAIL;
		}
		if (Map.CurrentMap.getObject(person.currentMapPos) is ResourceTile)
		{
			ResourceTile tile = (ResourceTile)Map.CurrentMap.getObject(person.currentMapPos);
			if(tile.m_resource == ResourceType.Timber
			   && person.Skills.Contains(Skill.Lumberjack))
			{
				person.Resources[ResourceType.Timber] += tile.GatherResource(ResourceType.Timber);
				person.SetBusy(5);
				person.ToDoList[0] = new Store();
				return ActionResult.CONTINUE; //Don't want to remove it.
			}
		}
		ResourceTile nearestResource = Map.CurrentMap.GetNearestResourceTile (person.currentMapPos, ResourceType.Timber);
		person.ToDoList.Insert (0, new Move (person.currentMapPos, nearestResource.m_MapPos));
		return ActionResult.CONTINUE;
	}
}

