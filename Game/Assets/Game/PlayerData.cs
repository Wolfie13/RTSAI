using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData {

    //resources
    public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();

    public List<Building> Buildings = new List<Building>();
    public List<Person> People = new List<Person>();


    public int TeamID;

    public PlayerData(int ID)
    {
        TeamID = ID;
        ResetResources();
    }


    private void ResetResources()
    {
        Resources.Clear();
        for (ResourceType i = ResourceType.Wood; i < ResourceType.NumOfResourcetypes; i++)
        {
            Resources.Add(i, 0);
        }
    }

    //people
    public List<Person> GetPeopleWithSkill(Skill wantedSkill)
    {
        List<Person> results = new List<Person>();

        foreach (var item in People)
        {
            if (item.Skills.Contains(wantedSkill))
                results.Add(item);
        }
        return results;
    }
    public List<Person> GetPeople() { return People; }
    public List<Person> GetPeopleAt(IVec2 MapPos)
    {
        List<Person> results = new List<Person>();

        foreach (var item in People)
        {
            if (item.currentMapPos == MapPos)
                results.Add(item);
        }
        return results;
    }

    public Person GetNonBusyPersonAt(IVec2 MapPos)
    {
        foreach (var item in GetPeopleAt(MapPos))
        {
            if (item.ToDoList.Count == 0 && item.BusyTime == 0)
                return item;
        }
        return null;
    }

    public List<Person> GetPeopleAtBuilding(Building b)
    {
        List<Person> result = new List<Person>();

        for (IVec2 offset = new IVec2(); offset.x < Building.Sizes[b.m_buildingtype].x; offset.x++)
        {
            for (offset.y = 0; offset.y < Building.Sizes[b.m_buildingtype].y; offset.y++)
            {
                result.AddRange(GetPeopleAt(b.m_MapPos + offset));
            }
        }

        return result;
    }

    //Buildings

    public List<Building> GetBuildingsOfType(BuildingType type)
    {
        List<Building> results = new List<Building>();

        foreach (var item in Buildings)
        {
            if (item.m_buildingtype == type)
                results.Add(item);
        }
        return results;
    }

    public List<Building> GetBuildings() { return Buildings; }
}
