﻿using UnityEngine;
using System.Collections;

public class Executive : MonoBehaviour {
	private Map map = null;
	private TaskPlanner taskPlanner = null;
	// Use this for initialization
	void Awake () {
		map = GameObject.FindObjectOfType<Map> ();
		if (map == null) {
			Debug.Log("Executive Failed to get map Handle and disabled itself.");
			this.enabled = false;
		}

		taskPlanner = new TaskPlanner ();
	}

	public void tick ()
    { 
		//innerAI (map.GetTeamData (0),  map.GetTeamData (1));
		//innerAI (map.GetTeamData (1),  map.GetTeamData (0));
		if (plannerTicks++ > 20) {
			Debug.Log("Planner Tick");
			taskPlanner.PlannerTick ();
			plannerTicks = 0;
		}
	}

	private int plannerTicks = 20;

	private void innerAI(PlayerData player, PlayerData opponent)
	{
		int ourArmyStrength = CountArmy (player);
		int theirArmyStrength = CountArmy (opponent);

		if (ourArmyStrength > theirArmyStrength + 10) {

		}
	}

	private static int CountArmy(PlayerData player)
	{
		int counter = 0;
		foreach (Person p in player.People) {
			if (p.Skills.Contains(Skill.Rifleman)){
				counter++;
			}
		}
		return counter;
	}

	void OnGUI() {
		Vector2 UIROOT = new Vector2 (10, 10);
		for (int i = 0; i != 2; i++) {
			PlayerData player = map.GetTeamData(i);
			if (player != null) {
				GUI.Label (new Rect(UIROOT.x, UIROOT.y, 400, 25), "Team " + i);
				GUI.Label (new Rect(UIROOT.x, UIROOT.y + 25, 400, 25), "Number of People: " + player.People.Count);
				GUI.Label (new Rect(UIROOT.x, UIROOT.y + 50, 400, 25), "Number of Buildings: " + player.Buildings.Count);
				GUI.Label (new Rect(UIROOT.x, UIROOT.y + 75, 400, 25), "Failed Orders: " + player.failedOrders);
				float Accum = 100;
				foreach (var v in player.Resources) {
					GUI.Label (new Rect(UIROOT.x, UIROOT.y + Accum, 400, 25), v.Key.ToString() + ": " + v.Value);
					Accum += 25;
				}
				UIROOT = UIROOT + new Vector2(200, 0);
			}
		}
	}
}
