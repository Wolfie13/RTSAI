using System;
public class Educate : Action
{
	public Educate(Skill skill, Person target, bool inSchool)
	{
		this.skill = skill;
		this.target = target;
        this.inSchool = inSchool ? StayInSchool.yes : StayInSchool.no;
	}

    public Educate(Skill skill, Person target)
    {
        this.skill = skill;
        this.target = target;
        this.inSchool = StayInSchool.whocares;
    }

    private enum StayInSchool
    {
        yes,
        no,
        whocares
    }

	private const int TRAIN_TIME = 10;
	private const int RIFLE_TRAIN_TIME = 30;

	private IVec2 mutualLearningZone = new IVec2(0, 0);
	private Skill skill;
	private Person target;
    private StayInSchool inSchool = StayInSchool.no;
	private bool taskDispatched = false;

	public override ActionResult actionTick (Person person)
	{
        if (target == null)
        {
            foreach (Person p in Map.CurrentMap.GetTeamData(person.teamID).GetPeople())
            {
                if (p == person) continue;

                if (p.ToDoList.Count == 0)
                {
                    target = p;
                }
            }
        }

		if (!taskDispatched) {
			mutualLearningZone = (person.currentMapPos + target.currentMapPos) / 2;
            
            var nearestSchool = Map.CurrentMap.GetTeamData(person.teamID).GetNearestBuilding(mutualLearningZone, BuildingType.School);
            if (nearestSchool == null)
            {
                inSchool = StayInSchool.no;
            }
            if (inSchool == StayInSchool.yes)
            {
                mutualLearningZone = nearestSchool.m_MapPos;
           	}
            if (inSchool == StayInSchool.whocares)
            {
                if ((mutualLearningZone - person.currentMapPos).magnitude() > (nearestSchool.m_MapPos - person.currentMapPos).magnitude())
                {
                    mutualLearningZone = nearestSchool.m_MapPos;
                    inSchool = StayInSchool.yes;
                }
                else
                {
                    inSchool = StayInSchool.no;
                }

            }

			target.ToDoList.Insert (0, new Learn (mutualLearningZone));
			taskDispatched = true;
		}

		if (skill == Skill.Rifleman) {

			if (Map.CurrentMap.getObject (person.currentMapPos) is Building) {
				Building CurrentBuilding = (Building)Map.CurrentMap.getObject (person.currentMapPos);
				
				var other = CurrentBuilding.GetNonBusyPersonInBuilding ();
				
				if (person.Skills.Contains (Skill.Rifleman)
					&& CurrentBuilding.m_buildingtype == BuildingType.Barracks
					&& CurrentBuilding.teamID == person.teamID
					&& other != null
					&& other.teamID == person.teamID) {
					person.SetBusy (RIFLE_TRAIN_TIME);
					other.SetBusy (RIFLE_TRAIN_TIME);
					other.Skills.Add (Skill.Rifleman);
					return ActionResult.SUCCESS;
				}
			}
		}

		if (inSchool == StayInSchool.yes) {
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

