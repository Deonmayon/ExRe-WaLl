using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuActionController : MonoBehaviour
{
    [SerializeField] GameObject SizeController;
    [SerializeField] GameObject PoseFigure;
    [SerializeField] GameObject Cursor;
    [SerializeField] GameObject ground;
    // Start is called before the first frame update
    public static string ActionMode = "idle";
    public static Vector3 cursor_positions;
    Vector3 worldPosition;
    Slider SizeControllerSlider;
    Collider planeCollide = null;

    public void SelectScale(){
        ActionMode = "scale";
        SizeController.SetActive(true);
    }
    public void SelectMove(){
        ActionMode = "move";
        SizeController.SetActive(false);
    }

    Vector3 GetMousePosition(){
        // this will return position of camera that you are controlling
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    void Start()
    {
        // for projecting position from screen to 3D
        planeCollide = ground.GetComponent<Collider>();
        ActionMode = "idle";
        // for setting size controller 
        SizeControllerSlider = SizeController.GetComponent<Slider>();
    }

    // Update is called once per frame

    void LateUpdate() {
        if(ActionMode == "scale"){
            float ScaleValue = SizeControllerSlider.value*10;
            PoseFigure.transform.localScale = new Vector3(ScaleValue,ScaleValue,ScaleValue);
        }
        else if(ActionMode == "move"){
            // get percentage of screen and project it to the plane
            Debug.LogFormat(" in world position{0}",cursor_positions);
            float x_position = -cursor_positions.x*planeCollide.bounds.size.x;
            // normalize lowwer bound
            x_position = Mathf.Max(x_position,ground.transform.position.x-planeCollide.bounds.size.x/2);
            // normalize upper bound
            x_position = Mathf.Min(x_position,ground.transform.position.x+planeCollide.bounds.size.x/2);
            worldPosition = new Vector3(
                x_position,
                0,
                PoseFigure.transform.position.z
              );
            PoseFigure.transform.position = worldPosition;
        }
    }
}
