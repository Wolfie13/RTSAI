using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float Scroll_Speed = 3;
    public float Scroll_Shift_Speed = 5;
    public float Camera_Zoom_Step = 2;

    //UI data
    public Vector2 UI_size = new Vector2(128f, 50f);
    public Vector2 UIPos = new Vector2(0.0f, 0.0f);
    public float UI_Distance = 10f;

    public float Min_Zoom_Dist = 2, Max_Zoom_Dist = 75;

    GameObject selectedObject = null;

    IVec2 MapPosClick = new IVec2();

    Map CurrentMap = null;

	// Use this for initialization
	void Start () {
        CurrentMap = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
	}
	
	// Update is called once per frame
	void Update () {


        //movement controls
        Vector3 movedirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        movedirection.Normalize();
        movedirection *= (Input.GetButton("CameraSpeedUp")) ?  Scroll_Shift_Speed : Scroll_Speed;
	
        transform.position += movedirection;

		
			camera.orthographicSize +=Input.GetAxis("Mouse ScrollWheel") * Camera_Zoom_Step;
			camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, Min_Zoom_Dist, Max_Zoom_Dist);
		

        transform.position = new Vector3(transform.position.x, 10, transform.position.z);

        
        //
        //object controls

        //select object
        if(Input.GetButtonDown("LeftClick"))
        {

            Vector3 MouseRawPos = Input.mousePosition;
            Ray mouseRay = camera.ScreenPointToRay(MouseRawPos);

            RaycastHit info;
            Physics.Raycast(mouseRay,out info);

            if(info.transform)
            {
                selectedObject = info.transform.gameObject;

            Debug.Log("Object selected: " + selectedObject.name);
            }
        }

        //select object
        if(Input.GetButtonDown("RightClick"))
        {

            Vector3 MouseRawPos = Input.mousePosition;
            Vector3 mouserealPos = camera.ScreenToWorldPoint(MouseRawPos);
            if (CurrentMap)
                MapPosClick = new IVec2(CurrentMap.getTileFromPos(mouserealPos));

            Debug.Log("click location: " + mouserealPos);
            Debug.Log("map location: " + MapPosClick.ToString());

            Debug.Log("thought location: " + CurrentMap.getTilePos(MapPosClick));


            if(selectedObject)
            {
                if(selectedObject.GetComponent<Person>())
                {
                    selectedObject.GetComponent<Person>().Move(MapPosClick);
                }
            }
        }


	
	}

    void OnGUI()
    {
        ////object info
      
        //int CurrentLine = 0;
        //GUI.BeginGroup(new Rect(Screen.width * UIPos.x, Screen.height * UIPos.y, Screen.width - Screen.width * UIPos.x, Screen.height - Screen.height * UIPos.y));
        //foreach (var item in Map.GlobalResources)
        //{
        //    AddLable(item.Key.ToString() + ": " + item.Value.ToString(), ref CurrentLine);
        //} 
           
        //GUI.EndGroup();
    }

    void AddLable(string Text, ref int CurrentLine)
    {
        GUI.Box(new Rect(0, CurrentLine * (UI_size.y + UI_Distance), UI_size.x, UI_size.y), Text);
        ++CurrentLine;
    }

    bool AddButton(string Text, ref int CurrentLine)
    {
       bool temp = GUI.Button(new Rect(0, CurrentLine * (UI_size.y + UI_Distance), UI_size.x, UI_size.y), Text);
        ++CurrentLine;

        return temp;
    }
}
