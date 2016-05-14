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
        {BuildingType.Sawmill,      new IVec2(3,3)},//may change
        {BuildingType.Blacksmith,   new IVec2(3,3)}//maychange
        //,{BuildingType.MarketStall,  new IVec2(1,1)}

    };


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

    [HideInInspector]
    public Dictionary<Resources, int> Resources = new Dictionary<Resources, int>();

    public BuildingType m_buildingtype;

    IVec2 m_buildingSize;


    //returns true if sucsessful
    public bool Build(BuildingType type, IVec2 mapPos)
    {
        bool building = false;
        m_buildingtype = type;
        switch (m_buildingtype)
        {
            case BuildingType.turfHut:
                building = CheckForSkillWithinBuilding(Skill.Labourer);
                break;
            case BuildingType.House:
                building = CheckResource(ResourceType.Stone);
                building = CheckResource(ResourceType.Wood);
                building = CheckForSkillWithinBuilding(Skill.Carpenter);
                building = CheckForSkillWithinBuilding(Skill.Labourer);
                break;
            case BuildingType.School:
                building = CheckResource(ResourceType.Stone);
                building = CheckResource(ResourceType.Wood);
                building = CheckResource(ResourceType.Iron);
                building = CheckForSkillWithinBuilding(Skill.Carpenter);
                building = CheckForSkillWithinBuilding(Skill.Labourer);
                break;
            case BuildingType.Barracks:
                building = CheckResource(ResourceType.Stone);
                building = CheckResource(ResourceType.Wood);
                building = CheckForSkillWithinBuilding(Skill.Carpenter);
                building = CheckForSkillWithinBuilding(Skill.Labourer);
                break;
            case BuildingType.Storage:
                building = CheckResource(ResourceType.Stone);
                building = CheckResource(ResourceType.Wood);
                building = CheckForSkillWithinBuilding(Skill.Carpenter);
                building = CheckForSkillWithinBuilding(Skill.Labourer);
                break;
            case BuildingType.Mine:
                building = CheckResource(ResourceType.Wood);
                building = CheckResource(ResourceType.Iron);
                building = CheckForSkillWithinBuilding(Skill.Blacksmith);
                building = CheckForSkillWithinBuilding(Skill.Carpenter);
                building = CheckForSkillWithinBuilding(Skill.Labourer);
                break;
            case BuildingType.Smelter:
                building = CheckResource(ResourceType.Stone);
                building = CheckForSkillWithinBuilding(Skill.Labourer);
                break;
            case BuildingType.Quarry:
                building = CheckForSkillWithinBuilding(Skill.Labourer);
                break;
            case BuildingType.Sawmill:
                building = CheckResource(ResourceType.Stone);
                building = CheckResource(ResourceType.Iron);
                building = CheckResource(ResourceType.Timber);
                building = CheckForSkillWithinBuilding(Skill.Labourer);
                break;
            case BuildingType.Blacksmith:
                building = CheckResource(ResourceType.Stone);
                building = CheckResource(ResourceType.Iron);
                building = CheckResource(ResourceType.Timber);
                building = CheckForSkillWithinBuilding(Skill.Labourer);
                break;
            default:
                break;
        }
        if (building)
        {
            
            //Spend Resource and set Busy timers
            //all require a labourer
            GetNonBusyPersonInBuildingWithSkill(Skill.Labourer).SetBusy(BuildTime[m_buildingtype]);

            switch (m_buildingtype)
            {
                case BuildingType.House:
                    Map.GlobalResources[ResourceType.Stone]--;
                    Map.GlobalResources[ResourceType.Wood]--;
                    GetNonBusyPersonInBuildingWithSkill(Skill.Carpenter).SetBusy(BuildTime[m_buildingtype]);
                    break;
                case BuildingType.School:
                    Map.GlobalResources[ResourceType.Stone]--;
                    Map.GlobalResources[ResourceType.Wood]--;
                    Map.GlobalResources[ResourceType.Iron]--;
                    GetNonBusyPersonInBuildingWithSkill(Skill.Carpenter).SetBusy(BuildTime[m_buildingtype]);
                    break;
                case BuildingType.Barracks:
                    Map.GlobalResources[ResourceType.Stone]--;
                    Map.GlobalResources[ResourceType.Wood]--;
                    GetNonBusyPersonInBuildingWithSkill(Skill.Carpenter).SetBusy(BuildTime[m_buildingtype]);
                    break;
                case BuildingType.Storage:
                    Map.GlobalResources[ResourceType.Stone]--;
                    Map.GlobalResources[ResourceType.Wood]--;
                    GetNonBusyPersonInBuildingWithSkill(Skill.Carpenter).SetBusy(BuildTime[m_buildingtype]);
                    break;
                case BuildingType.Mine:
                    Map.GlobalResources[ResourceType.Wood]--;
                    Map.GlobalResources[ResourceType.Iron]--;
                    GetNonBusyPersonInBuildingWithSkill(Skill.Carpenter).SetBusy(BuildTime[m_buildingtype]);
                    GetNonBusyPersonInBuildingWithSkill(Skill.Blacksmith).SetBusy(BuildTime[m_buildingtype]);
                    break;
                case BuildingType.Smelter:
                    Map.GlobalResources[ResourceType.Stone]--;
                    break;
                case BuildingType.Sawmill:
                    Map.GlobalResources[ResourceType.Stone]--;
                    Map.GlobalResources[ResourceType.Iron]--;
                    Map.GlobalResources[ResourceType.Timber]--;
                    break;
                case BuildingType.Blacksmith:
                    Map.GlobalResources[ResourceType.Stone]--;
                    Map.GlobalResources[ResourceType.Iron]--;
                    Map.GlobalResources[ResourceType.Timber]--;

                    break;
                default:
                    break;
            }
        }
        return building;
    }

    bool CheckResource(ResourceType type)
    {
          if (Map.GlobalResources[type] > 0)
          {
              return true;
          }
        return false;
    }

    bool CheckForSkillWithinBuilding(Skill WantedSkill)
    {
        bool Skillfound = false;

        for (IVec2 offset = new IVec2(); offset.x < Building.Sizes[m_buildingtype].x; offset.x++)
        {
            for (offset.y = 0; offset.y < Building.Sizes[m_buildingtype].y; offset.y++)
            {
                 foreach (var item in Map.CurrentMap.GetPeopleAt(m_MapPos + offset))
                {
                    if (item.Skills.Contains(WantedSkill))
                    {
                        Skillfound = true;
                        break;
                    }
                }
                 if (Skillfound)
                     break;
            }
            if (Skillfound)
                break;
        }
        return Skillfound;
        
    }

    Person GetNonBusyPersonInBuildingWithSkill(Skill wantedSkill)
    {
        Person john = null;

        for (IVec2 offset = new IVec2(); offset.x < Building.Sizes[m_buildingtype].x; offset.x++)
        {
            for (offset.y = 0; offset.y < Building.Sizes[m_buildingtype].y; offset.y++)
            {
                foreach (var item in Map.CurrentMap.GetPeopleAt(m_MapPos + offset))
                {
                    if (item.Skills.Contains(wantedSkill) && item.ToDoList.Count == 0)
                    {
                        john = item;
                        break;
                    }
                }
                if (john)
                    break;
            }
            if (john)
                break;
        }

        return john;

    }

    public Person GetNonBusyPersonInBuilding()
    {
        Person john = null;

        for (IVec2 offset = new IVec2(); offset.x < Building.Sizes[m_buildingtype].x; offset.x++)
        {
            for (offset.y = 0; offset.y < Building.Sizes[m_buildingtype].y; offset.y++)
            {
                foreach (var item in Map.CurrentMap.GetPeopleAt(m_MapPos + offset))
                {
                    if (item.ToDoList.Count == 0 && item.BusyTime == 0)
                    {
                        john = item;
                        break;
                    }
                }
                if (john)
                    break;
            }
            if (john)
                break;
        }

        return john;
    }
}
