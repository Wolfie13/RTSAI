using System;
public class Educate : Action
{
	public Educate(Skill skill, Person target, bool inSchool)
	{
		this.skill = skill;
		this.target = target;
		this.inSchool = inSchool;
	}

	private const int TRAIN_TIME = 10;
	private const int RIFLE_TRAIN_TIME = 30;

	private IVec2 mutualLearningZone = new IVec2(0, 0);
	private Skill skill;
	private Person target;
	private bool inSchool = false;
	private bool taskDispatched = false;

	public override ActionResult actionTick (Person person)
	{
		if (!taskDispatched) {
			mutualLearningZone = (person.currentMapPos + target.currentMapPos) / 2;
			if (inSchool) {
				mutualLearningZone = Map.CurrentMap.GetNearestBuilding(mutualLearningZone, BuildingType.School).m_MapPos;
           	}

			target.ToDoList.Insert (0, new Learn (mutualLearningZone));
			taskDispatched = true;
		}


		if (Map.CurrentMap.getObject(person.currentMapPos) is Building)
		{
			Building CurrentBuilding = (Building)Map.CurrentMap.getObject(person.currentMapPos);
			
			var other = CurrentBuilding.GetNonBusyPersonInBuilding();
			
			if (person.Skills.Contains(Skill.Rifleman)
			    && CurrentBuilding.m_buildingtype == BuildingType.Barracks
			    && CurrentBuilding.teamID == person.teamID
			    && other != null
			    && other.teamID == person.teamID)
			{
				person.SetBusy(RIFLE_TRAIN_TIME);
				other.SetBusy(RIFLE_TRAIN_TIME);
				other.Skills.Add(Skill.Rifleman);
				return ActionResult.SUCCESS;
			}
		}

		if (inSchool) {
			if (Map.CurrentMap.getObject(person.currentMapPos) is Building)
			{
				Building CurrentBuilding = (Building)Map.CurrentMap.getObject(person.currentMapPos);
				
				if (CurrentBuilding.m_buildingtype == BuildingType.School
				    && CurrentBuilding.teamID == person.teamID)
				{
					var other = CurrentBuilding.GetNonBusyPersonInBuilding();
					while (other != null && other.teamID == person.teamID)
					{
						foreach (var item in person.Skills)
						{
							if (!other.Skills.Contains(item))
							{
								other.SetBusy(50);
								person.SetBusy(50);
								other.Skills.Add(item);
								return ActionResult.SUCCESS;
							}
						}
						other = CurrentBuilding.GetNonBusyPersonInBuilding();
					}
				}
				
			}
		} else {
			if (target == null || target.teamID != person.teamID)
				return ActionResult.FAIL;

			if (target.currentMapPos == person.currentMapPos) {
				if(!target.Skills.Contains(skill))
				{
					target.SetBusy(TRAIN_TIME);
					person.SetBusy(TRAIN_TIME);
					target.transform.Translate(1.5f, 0, 0);
					person.transform.Translate(-1.5f, 0, 0);
					target.Skills.Add(skill);
					return ActionResult.SUCCESS;
				}
			}
		}

		person.ToDoList.Insert (0, new Move (person.currentMapPos, mutualLearningZone));
		return ActionResult.CONTINUE;
	}
}

