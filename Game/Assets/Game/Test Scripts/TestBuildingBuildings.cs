using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestBuildingBuildings : MonoBehaviour {

    public int GlobleWood = 0,
               GlobleTimber = 0,
               GlobleStone = 0,
               GlobleOre = 0,
               GlobleIron = 0,
               GlobleCoal = 0;

    public int BuildX = 0, BuildY = 0;
     
    public int NumberOfPeople = 0;
    public List<Skill> PeopleSkils = new List<Skill>();

    public float DelayTime = 1f;

    public BuildingType type;
    public int teamID = 0;




	// Use this for initialization
	IEnumerator Start () 
    {
        yield return new WaitForSeconds(DelayTime);

        IVec2 MapPos = new IVec2(BuildX, BuildY);

        for (int i = 0; i < NumberOfPeople; i++)
        {
             Map.CurrentMap.AddPerson(MapPos, teamID);
        }
        foreach (var item in Map.CurrentMap.GetPeopleAt(MapPos))
        {
            if (NumberOfPeople > 0 && item.teamID == teamID)
            {
                item.Skills.AddRange(PeopleSkils);
                NumberOfPeople--;
            }
        }

        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Coal] = GlobleCoal;
        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Iron] = GlobleIron;
        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Ore] = GlobleOre;
        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Stone] = GlobleStone;
        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Timber] = GlobleTimber;
        Map.CurrentMap.GetTeamData(teamID).Resources[ResourceType.Wood] = GlobleWood;

        if (!Map.CurrentMap.BuildBuilding(type, MapPos, teamID))
        {
            Debug.Log("Building couldn't be built");
        }
	}
	
}
