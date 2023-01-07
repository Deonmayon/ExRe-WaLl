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
    [Space]
    [SerializeField] GameObject action_menu_bg;
    [SerializeField] GameObject action_cursor;
    [SerializeField] GameObject[] action_buttons;
    [Space]
    [SerializeField] GameObject Progress_bar;
    Slider _progress;

    public static string ActionMode = "idle";
    float[] index_values = new float[]{0.70f, 0.50f, 0.30f};
    float diff, max_distance = 1f, count_pause = 0f;
    int Selected_idx = 4;
    
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

    public void SelectSelect(){
        ActionMode = "select";
        SizeController.SetActive(false);
    }

    Vector3 GetMousePosition(){
        // this will return position of camera that you are controlling
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    bool CircularProgress(float timer){
        count_pause += Time.deltaTime;
        _progress.value = count_pause / timer;
        if (count_pause >= timer){
            count_pause = 0f;
            return true;
        }
        return false;
    }

    void Start()
    {
        // for projecting position from screen to 3D
        planeCollide = ground.GetComponent<Collider>();
        ActionMode = "idle";
        // for setting size controller 
        SizeControllerSlider = SizeController.GetComponent<Slider>();
        _progress = Progress_bar.GetComponent<Slider>();
    }

    // Update is called once per frame

    void LateUpdate() {
        Debug.LogFormat("{0}",ActionMode);
        // Debug.LogFormat("{0}",cursor_positions);
        if(ActionMode == "idle"){          
            // see if hand is on the right side of the screen
            if(cursor_positions.x <-0.4f){
                // visualize menu is selected
                action_menu_bg.GetComponent<Image>().color = new Color32(0, 255, 180, 100);
                // for selecting which menu hand point on
                max_distance = 1f;
                Debug.LogFormat("{0}",cursor_positions);
                 for(int i=0; i < index_values.Length; i++){
                    diff = Mathf.Abs(index_values[i] - (cursor_positions.y + 0.5f));
                    if(max_distance > diff){
                        max_distance = diff;
                        Selected_idx = i;
                    }
                }
                // visualize selected menu
                action_cursor.SetActive(true);
                action_cursor.transform.position = action_buttons[Selected_idx].transform.position;
                count_pause = 0f;
            } else if (cursor_positions.x > 0.1f){
                // hand go left to select
                // circular progress bar
                if (CircularProgress(0.5f)){
                     // changing action mode
                    switch (Selected_idx)
                    {
                        case 0:
                            SelectSelect();
                            break;
                        case 1:
                            SelectMove();
                            break;
                        case 2:
                            SelectScale();
                            break;
                        default:
                            break;
                    };
                    action_menu_bg.GetComponent<Image>().color = new Color32(255, 255, 225, 100);
                    action_cursor.GetComponent<Image>().color = new Color32(240, 190, 0, 200);
                    _progress.value = 0;
                }
            }
            else{
                count_pause = 0f;
            }
        }
        else if(ActionMode == "scale"){
            float ScaleValue = SizeControllerSlider.value*1;
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
