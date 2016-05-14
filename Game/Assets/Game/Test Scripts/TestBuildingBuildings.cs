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




	// Use this for initialization
	IEnumerator Start () 
    {
        yield return new WaitForSeconds(DelayTime);

        IVec2 MapPos = new IVec2(BuildX, BuildY);

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

        Map.CurrentMap.BuildBuilding(type, MapPos);
	}
	
}
