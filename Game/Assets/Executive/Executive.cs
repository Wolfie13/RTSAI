using UnityEngine;
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
	void Update () 7

	}

	private static Vector2 UIROOT = new Vector2(10, 10);

	void OnGUI() {
		GUI.Label (new Rect(UIROOT.x, UIROOT.y, 400, 25), "Number of People: " + map.GetPeople().Count);
		GUI.Label (new Rect(UIROOT.x, UIROOT.y + 25, 400, 25), "Number of Buildings: " + map.GetBuildings().Count);
	}
}
