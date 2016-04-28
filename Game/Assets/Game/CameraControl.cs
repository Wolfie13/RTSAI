using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float Scroll_Speed = 3;
    public float Scroll_Shift_Speed = 5;
    public float Camera_Zoom_Step = 2;

    public float Min_Zoom_Dist = 2, Max_Zoom_Dist = 75;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 movedirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        movedirection.Normalize();
        movedirection *= (Input.GetButton("CameraSpeedUp")) ?  Scroll_Shift_Speed : Scroll_Speed;

        movedirection.y = Input.GetAxis("Mouse ScrollWheel") * Camera_Zoom_Step;

        transform.position += movedirection;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, Min_Zoom_Dist, Max_Zoom_Dist), transform.position.z);

        


	
	}
}
