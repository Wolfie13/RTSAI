﻿using UnityEngine;
using System.Collections;

public class Executive : MonoBehaviour {
	private Map map = null;
	// Use this for initialization
	void Awake () {
		map = GameObject.FindObjectOfType<Map> ();
		if (map == null) {
			Debug.Log("Executive Failed to get map Handle and disabled itself.");
			this.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
    { 

	}

	void OnGUI() {
		Vector2 UIROOT = new Vector2 (10, 10);
		for (int i = 0; i != 2; i++) {
			PlayerData player = map.GetTeamData(i);
			if (player != null) {
				GUI.Label (new Rect(UIROOT.x, UIROOT.y, 400, 25), "Team " + i);
				GUI.Label (new Rect(UIROOT.x, UIROOT.y + 25, 400, 25), "Number of People: " + player.People.Count);
				GUI.Label (new Rect(UIROOT.x, UIROOT.y + 50, 400, 25), "Number of Buildings: " + player.Buildings.Count);
				UIROOT = UIROOT + new Vector2(200, 0);
			}
		}
	}
}
