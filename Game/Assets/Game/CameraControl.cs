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

    GameObject selectedObject = null, interactobject = null;

    bool display_context_menu = false;

    ivec2 MapPosClick = new ivec2();

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
            interactobject = null;
        }

        //select object
        if(Input.GetButtonDown("RightClick"))
        {

            Vector3 MouseRawPos = Input.mousePosition;
            Vector3 mouserealPos = camera.ScreenToWorldPoint(MouseRawPos);
            if (CurrentMap)
                MapPosClick = CurrentMap.getTileFromPos(mouserealPos);


            if(selectedObject)
            {
                if(selectedObject.GetComponent<Person>())
                {
                     Ray mouseRay = camera.ScreenPointToRay(MouseRawPos);

                    RaycastHit info;
                    Physics.Raycast(mouseRay,out info);
                    if (info.transform)
                        interactobject = info.transform.gameObject;

                    display_context_menu = true;
                    selectedObject.GetComponent<Person>().Move(MapPosClick, ()=>{});
                }
            }

            Debug.Log("click location: " + mouserealPos);
            Debug.Log("map location: " + MapPosClick.ToString());

            Debug.Log("thought location: " + CurrentMap.getTilePos(MapPosClick));



        }


	
	}

    void onGUI()
    {
        //object info
        if(selectedObject)
        {
            int CurrentLine = 0;
            GUI.BeginGroup(new Rect(Screen.width * UIPos.x, Screen.height * UIPos.y, Screen.width - Screen.width * UIPos.x, Screen.height - Screen.height * UIPos.y));
            if (selectedObject.GetComponent<Person>())
            {
               
                foreach (var item in selectedObject.GetComponent<Person>().Resources)
                {
                    AddLable(item.Key.ToString() + ": " + item.Value.ToString(), ref CurrentLine);
                }
                foreach (var item in selectedObject.GetComponent<Person>().Skills)
                {
                    AddLable(item.ToString(), ref CurrentLine);
                }
            }
            else if (selectedObject.GetComponent<Building>())
            {
                AddLable(selectedObject.GetComponent<Building>().m_buildingtype.ToString(), ref CurrentLine);

                foreach (var item in selectedObject.GetComponent<Building>().Resources)
                {
                    AddLable(item.Key.ToString() + ": " + item.Value.ToString(), ref CurrentLine);
                }
            }
            GUI.EndGroup();

            //contex menu
            if(display_context_menu)
            {   
                CurrentLine = 0;
                GUI.BeginGroup(new Rect(Screen.width * UIPos.x + (UI_Distance *5), Screen.height * UIPos.y, Screen.width - Screen.width * UIPos.x, Screen.height - Screen.height * UIPos.y));
                if (selectedObject.GetComponent<Person>())
                {

                    if(interactobject && interactobject.GetComponent<Building>())
                    {
                        Building currentBuilding = interactobject.GetComponent<Building>();

                        switch (currentBuilding.m_buildingtype)
                        {
                            case BuildingType.turfHut:
                                if (currentBuilding.people.Count > 0)
                                    if(AddButton("Famliy", ref CurrentLine))
                                        selectedObject.GetComponent<Person>().Move(currentBuilding.m_MapPos,()=>{/*famliy methord here*/});
                                break;
                            case BuildingType.House:
                                 if (currentBuilding.people.Count > 0)
                                    if(AddButton("Famliy", ref CurrentLine))
                                        selectedObject.GetComponent<Person>().Move(currentBuilding.m_MapPos,()=>{/*famliy methord here*/});
                                break;
                            case BuildingType.School:
                                 if (currentBuilding.people.Count > 0)
                                    if(AddButton("Educate", ref CurrentLine))
                                        selectedObject.GetComponent<Person>().Move(currentBuilding.m_MapPos,()=>{/*Educate methord here*/});
                                break;
                            case BuildingType.Barracks:
                                if (currentBuilding.people.Count > 0)
                                    if (AddButton("Train", ref CurrentLine))
                                        selectedObject.GetComponent<Person>().Move(currentBuilding.m_MapPos, () => {/*Train methord here*/});
                                break;
                            case BuildingType.Storage:
                                if (AddButton("Store", ref CurrentLine))
                                    selectedObject.GetComponent<Person>().Move(currentBuilding.m_MapPos, () => {/*Store methord here*/});
                                break;
                            case BuildingType.Mine:
                                if (AddButton("Mine", ref CurrentLine))
                                    selectedObject.GetComponent<Person>().Move(currentBuilding.m_MapPos, () => {/*Mine methord here*/});
                                break;
                            case BuildingType.Smelter:
                                if (AddButton("Smelt", ref CurrentLine))
                                    selectedObject.GetComponent<Person>().Move(currentBuilding.m_MapPos, () => {/*Smelt methord here*/});
                                break;
                            case BuildingType.Quarry:
                                if (AddButton("Quarry", ref CurrentLine))
                                    selectedObject.GetComponent<Person>().Move(currentBuilding.m_MapPos, () => {/*Quarry methord here*/});
                                break;
                            case BuildingType.Sawmill:
                                if (AddButton("Saw Wood", ref CurrentLine))
                                    selectedObject.GetComponent<Person>().Move(currentBuilding.m_MapPos, () => {/*Saw methord here*/});
                                break;
                            case BuildingType.Blacksmith:
                                if (AddButton("Make Tool", ref CurrentLine))
                                    selectedObject.GetComponent<Person>().Move(currentBuilding.m_MapPos, () => {/*Blacksmith methord here*/});
                                break;
                            case BuildingType.MarketStall:
                                break;
                            default:
                                break;
                        }
                    }

                }
                GUI.EndGroup();
            }

        }
        


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
