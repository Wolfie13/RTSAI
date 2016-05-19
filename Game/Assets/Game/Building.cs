using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BuildingType
{
    none,
    turfHut,
    House,
    School,
    Barracks,
    Storage,
    Mine,
    Smelter,
    Quarry,
    Sawmill,
    Blacksmith
    //,MarketStall
}



public class Building : MapObject {

    public static Dictionary<BuildingType, IVec2> Sizes = new Dictionary<BuildingType, IVec2>()
    {
        {BuildingType.none,         new IVec2(1,1)},
        {BuildingType.turfHut,      new IVec2(2,2)},
        {BuildingType.House,        new IVec2(2,3)},
        {BuildingType.School,       new IVec2(5,5)},
        {BuildingType.Barracks,     new IVec2(5,5)},
        {BuildingType.Storage,      new IVec2(5,5)},
        {BuildingType.Mine,         new IVec2(1,1)},
        {BuildingType.Smelter,      new IVec2(2,2)},
        {BuildingType.Quarry,       new IVec2(1,1)},
        {BuildingType.Sawmill,      new IVec2(2,2)},
        {BuildingType.Blacksmith,   new IVec2(2,2)}
        //,{BuildingType.MarketStall,  new IVec2(1,1)}

    };

	public override void tick() {

	}


    public static Dictionary<BuildingType, int> BuildTime = new Dictionary<BuildingType, int>()
    {
        {BuildingType.none,         0},
        {BuildingType.turfHut,     10},
        {BuildingType.House,       15},
        {BuildingType.School,      30},
        {BuildingType.Barracks,    30},
        {BuildingType.Storage,     20},
        {BuildingType.Mine,        50},
        {BuildingType.Smelter,     20},
        {BuildingType.Quarry,      10},
        {BuildingType.Sawmill,     30},
        {BuildingType.Blacksmith,  30}
       // ,{BuildingType.MarketStall,  5}

    };

    public BuildingType m_buildingtype;

    IVec2 m_buildingSize;

    public int teamID;
    public string name;


    //returns true if sucsessful
    public bool Build(BuildingType type, IVec2 mapPos, int ID)
    {
        teamID = ID;
        bool building = true;
        m_buildingtype = type;
        m_MapPos = mapPos;
        switch (m_buildingtype)
        {
            case BuildingType.turfHut:
                if(!CheckForSkillWithinBuilding(Skill.Labourer)) return false;
                break;
            case BuildingType.House:
                if(!CheckResource(ResourceType.Stone)) return false;
                if(!CheckResource(ResourceType.Wood)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Carpenter)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Labourer)) return false;
                break;
            case BuildingType.School:
                if(!CheckResource(ResourceType.Stone)) return false;
                if(!CheckResource(ResourceType.Wood)) return false;
                if(!CheckResource(ResourceType.Iron)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Carpenter)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Labourer)) return false;
                break;
            case BuildingType.Barracks:
                if(!CheckResource(ResourceType.Stone)) return false;
                if(!CheckResource(ResourceType.Wood)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Carpenter)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Labourer)) return false;
                break;
            case BuildingType.Storage:
                if(!CheckResource(ResourceType.Stone)) return false;
                if(!CheckResource(ResourceType.Wood)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Carpenter)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Labourer)) return false;
                break;
            case BuildingType.Mine:
                if(!CheckResource(ResourceType.Wood)) return false;
                if(!CheckResource(ResourceType.Iron)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Blacksmith)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Carpenter)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Labourer)) return false;
                break;
            case BuildingType.Smelter:
                if(!CheckResource(ResourceType.Stone)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Labourer)) return false;
                break;
            case BuildingType.Quarry:
                if(!CheckForSkillWithinBuilding(Skill.Labourer)) return false;
                break;
            case BuildingType.Sawmill:
                if(!CheckResource(ResourceType.Stone)) return false;
                if(!CheckResource(ResourceType.Iron)) return false;
                if(!CheckResource(ResourceType.Timber)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Labourer)) return false;
                break;
            case BuildingType.Blacksmith:
                if(!CheckResource(ResourceType.Stone)) return false;
                if(!CheckResource(ResourceType.Iron)) return false;
                if(!CheckResource(ResourceType.Timber)) return false;
                if(!CheckForSkillWithinBuilding(Skill.Labourer)) return false;
                break;
            default:
                building = false;
                break;
        }
        if (building)
        {
            
            //Spend Resource and set Busy timers
            //all require a labourer
            GetNonBusyPersonInBuildingWithSkill(Skill.Labourer).SetBusy(BuildTime[m_buildingtype]);
            PlayerData CurrentTeamData = Map.CurrentMap.GetTeamData(teamID);
            switch (m_buildingtype)
            {
                case BuildingType.House:
                    CurrentTeamData.Resources[ResourceType.Stone]--;
                    CurrentTeamData.Resources[ResourceType.Wood]--;
                    GetNonBusyPersonInBuildingWithSkill(Skill.Carpenter).SetBusy(BuildTime[m_buildingtype]);
                    break;
                case BuildingType.School:
                    CurrentTeamData.Resources[ResourceType.Stone]--;
                    CurrentTeamData.Resources[ResourceType.Wood]--;
                    CurrentTeamData.Resources[ResourceType.Iron]--;
                    GetNonBusyPersonInBuildingWithSkill(Skill.Carpenter).SetBusy(BuildTime[m_buildingtype]);
                    break;
                case BuildingType.Barracks:
                    CurrentTeamData.Resources[ResourceType.Stone]--;
                    CurrentTeamData.Resources[ResourceType.Wood]--;
                    GetNonBusyPersonInBuildingWithSkill(Skill.Carpenter).SetBusy(BuildTime[m_buildingtype]);
                    break;
                case BuildingType.Storage:
                    CurrentTeamData.Resources[ResourceType.Stone]--;
                    CurrentTeamData.Resources[ResourceType.Wood]--;
                    GetNonBusyPersonInBuildingWithSkill(Skill.Carpenter).SetBusy(BuildTime[m_buildingtype]);
                    break;
                case BuildingType.Mine:
                    CurrentTeamData.Resources[ResourceType.Wood]--;
                    CurrentTeamData.Resources[ResourceType.Iron]--;
                    GetNonBusyPersonInBuildingWithSkill(Skill.Carpenter).SetBusy(BuildTime[m_buildingtype]);
                    GetNonBusyPersonInBuildingWithSkill(Skill.Blacksmith).SetBusy(BuildTime[m_buildingtype]);
                    break;
                case BuildingType.Smelter:
                    CurrentTeamData.Resources[ResourceType.Stone]--;
                    break;
                case BuildingType.Sawmill:
                    CurrentTeamData.Resources[ResourceType.Stone]--;
                    CurrentTeamData.Resources[ResourceType.Iron]--;
                    CurrentTeamData.Resources[ResourceType.Timber]--;
                    break;
                case BuildingType.Blacksmith:
                    CurrentTeamData.Resources[ResourceType.Stone]--;
                    CurrentTeamData.Resources[ResourceType.Iron]--;
                    CurrentTeamData.Resources[ResourceType.Timber]--;

                    break;
                default:
                    break;
            }
        }
        return building;
    }

    bool CheckResource(ResourceType type)
    {
          if (Map.CurrentMap.GetTeamData(teamID).Resources[type] > 0)
          {
              return true;
          }
        return false;
    }

    bool CheckForSkillWithinBuilding(Skill WantedSkill)
    {
		IVec2 offset = new IVec2();
		IVec2 size = Building.Sizes[m_buildingtype];
		for (offset.x = -size.x / 2; offset.x < size.x / 2; offset.x++) {
			for (offset.y = -size.y / 2; offset.y < size.y / 2; offset.y++) {
                 foreach (var item in Map.CurrentMap.GetPeopleAt(m_MapPos + offset))
                {
                    if (item.Skills.Contains(WantedSkill) && item.teamID == teamID)
                    {
						return true;
                    }
                }
            }
        }
		return false;
    }

    Person GetNonBusyPersonInBuildingWithSkill(Skill wantedSkill)
    {
		IVec2 offset = new IVec2();
		IVec2 size = Building.Sizes[m_buildingtype];
		for (offset.x = -size.x / 2; offset.x < size.x / 2; offset.x++) {
			for (offset.y = -size.y / 2; offset.y < size.y / 2; offset.y++) {
                foreach (var item in Map.CurrentMap.GetPeopleAt(m_MapPos + offset))
                {
                    if (item.Skills.Contains(wantedSkill) && item.ToDoList.Count == 0 && item.teamID == teamID)
                    {
						return item;
                    }
                }
            }
        }
		return null;
    }

	public List<Person> GetPeopleInBuilding()
	{
		List<Person> result = new List<Person>();
		IVec2 offset = new IVec2();
		IVec2 size = Building.Sizes[m_buildingtype];
		for (offset.x = -size.x / 2; offset.x < size.x / 2; offset.x++) {
			for (offset.y = -size.y / 2; offset.y < size.y / 2; offset.y++) {
				foreach (var item in Map.CurrentMap.GetPeopleAt(m_MapPos + offset))
				{
					if (item.teamID == teamID)
					{
						result.Add(item);
					}
				}
			}
		}
		
		return result;
	}

    public Person GetNonBusyPersonInBuilding()
    {

		IVec2 offset = new IVec2();
		IVec2 size = Building.Sizes[m_buildingtype];
		for (offset.x = -size.x / 2; offset.x < size.x / 2; offset.x++) {
			for (offset.y = -size.y / 2; offset.y < size.y / 2; offset.y++) {
                foreach (var item in Map.CurrentMap.GetPeopleAt(m_MapPos + offset))
                {
                    if (item.ToDoList.Count == 0 && item.BusyTime == 0 && item.teamID == teamID)
                    {
						return item;
                    }
                }
            }
        }
		return null;
    }
}
