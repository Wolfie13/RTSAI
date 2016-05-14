using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestAction : MonoBehaviour {

    public int GlobleWood = 0,
               GlobleTimber = 0,
               GlobleStone = 0,
               GlobleOre = 0,
               GlobleIron = 0,
               GlobleCoal = 0;

    public int SpawnX = 0, SpawnY = 0;

    public int NumberOfPeople = 0;
    public List<Skill> PeopleSkils = new List<Skill>();

    public float DelayTime = 1f;

    public List<action> ActToDo = new List<action>();




    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(DelayTime);

        IVec2 MapPos = new IVec2(SpawnX, SpawnY);

        for (int i = 0; i < NumberOfPeople; i++)
        {
            Map.CurrentMap.AddPerson(MapPos);
        }
        foreach (var item in Map.CurrentMap.GetPeopleAt(MapPos))
        {
            item.Skills.AddRange(PeopleSkils);
        }

        Map.GlobalResources[ResourceType.Coal] = GlobleCoal;
        Map.GlobalResources[ResourceType.Iron] = GlobleIron;
        Map.GlobalResources[ResourceType.Ore] = GlobleOre;
        Map.GlobalResources[ResourceType.Stone] = GlobleStone;
        Map.GlobalResources[ResourceType.Timber] = GlobleTimber;
        Map.GlobalResources[ResourceType.Wood] = GlobleWood;

        Map.CurrentMap.GetPeopleAt(MapPos)[0].ToDoList.AddRange(ActToDo);
    }
}
