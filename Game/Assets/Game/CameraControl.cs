using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float Scroll_Speed = 3;
    public float Scroll_Shift_Speed = 5;
    public float Camera_Zoom_Step = 2;

    public float Min_Zoom_Dist = 2, Max_Zoom_Dist = 75;

	private Camera thisCamera = null;

	// Use this for initialization
	void Start () {
		thisCamera = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 movedirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        movedirection.Normalize();
        movedirection *= (Input.GetButton("CameraSpeedUp")) ?  Scroll_Shift_Speed : Scroll_Speed;
	
        transform.position += movedirection;

		if (thisCamera) 
		{
			thisCamera.orthographicSize +=Input.GetAxis("Mouse ScrollWheel") * Camera_Zoom_Step;
			thisCamera.orthographicSize = Mathf.Clamp(thisCamera.orthographicSize, Min_Zoom_Dist, Max_Zoom_Dist);
		}

        transform.position = new Vector3(transform.position.x, 10, transform.position.z);

        


	
	}
}
