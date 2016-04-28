using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float Scroll_Speed = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 movedirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        movedirection.Normalize();
        movedirection *= Scroll_Speed;

        transform.position += movedirection;
	
	}
}
