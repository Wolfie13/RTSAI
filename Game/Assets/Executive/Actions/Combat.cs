using System;
using System.Collections.Generic;

public class Combat : Action
{

	public override ActionResult actionTick (Person person)
	{
		if (person.Skills.Contains(Skill.Rifleman))
		{
			List<Person> others = new List<Person>();
			for(IVec2 offset = new IVec2(-5,-5); offset.x < 6;++offset.x)
			{
				for(offset.y = -5; offset.y < 6;++offset.y)
				{
					if(offset.magnitude() <= 5)
					{
						others.AddRange(Map.CurrentMap.GetPeopleAt(person.currentMapPos + offset));
					}
				}
			}
			Person other = null;
			foreach (var item in others)
			{
				if(item.teamID != person.teamID)
				{
					other = item;
					break;
				}
			}
			if(other)
			{
				if(other.Skills.Contains(Skill.Rifleman) && UnityEngine.Random.Range(0,100) < (100.0f/3f * 2f))
				{
					Map.CurrentMap.KillPerson(person);
				}
				else
				{
					Map.CurrentMap.KillPerson(other);
					person.SetBusy(1);
				}
			}
		}
		return ActionResult.FAIL;
	}
}

