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

    public List<Action> ActToDo = new List<Action>();

    public int teamID = 0;




    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(DelayTime);

        IVec2 MapPos = new IVec2(SpawnX, SpawnY);

        for (int i = 0; i < NumberOfPeople; i++)
        {
            Map.CurrentMap.AddPerson(MapPos, teamID);
        }
        foreach (var item in Map.CurrentMap.GetPeopleAt(MapPos))
        {
            if(item.teamID == teamID)
                item.Skills.AddRange(PeopleSkils);
        }

        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Coal] = GlobleCoal;
        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Iron] = GlobleIron;
        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Ore] = GlobleOre;
        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Stone] = GlobleStone;
        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Timber] = GlobleTimber;
        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Wood] = GlobleWood;

        Map.CurrentMap.GetPeopleAt(MapPos)[0].ToDoList.AddRange(ActToDo);
    }
}
