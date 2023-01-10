using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseMenuController : MonoBehaviour
{
    [SerializeField] GameObject[] PoseButtons;
    [SerializeField] GameObject[] PoseCollection;
    GameObject PoseTemp;
    [Space]
    [SerializeField] GameObject pose_menu_bg;
    [SerializeField] GameObject pose_cursor;
    [Space]
    [SerializeField] GameObject Progress_bar;
    Slider _progress;
    Vector3 PointerPosition;
    int Selected_idx;
    float[] index_values = new float[]{0.70f, 0.50f, 0.30f};
    float diff, max_distance = 1,count_pause = 0f;
    public static bool[] fingerups = new bool[]{true, true, true, true, true};
    bool start_hover = false;

    public static bool IsFisting(){
        // check every finger except for thumb
        if (!(fingerups[1] && fingerups[2] && fingerups[3] && fingerups[4])) return true;
        return false;
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
        _progress = Progress_bar.GetComponent<Slider>();
        PoseTemp = PoseCollection[0];
        // PoseTemp.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.LogFormat("{0} {1} {2} {3} {4}",fingerups[0],fingerups[1],fingerups[2],fingerups[3],fingerups[4]);
        // if Action mode is Idle then we can select the pose from the nearest x value
        if (menuActionController.ActionMode == "select"){
            // get hand location
            PointerPosition = menuActionController.cursor_positions;
            Debug.LogFormat("{0}",PointerPosition);
            // check y position if It is hovering at pose menu
            if (PointerPosition.y >= 0.25){
                // visualizing selected menu (may be glowing panel)
                pose_menu_bg.GetComponent<Image>().color = new Color32(0, 255, 180, 100);
                // reset in tokens
                max_distance = 1;
                //flag selected
                start_hover = true;
                //check the x position and determine which menu it is selecting
                
                for(int i=0; i < PoseCollection.Length; i++){
                    diff = Mathf.Abs(index_values[i] - (PointerPosition.x + 1f)/2);
                    if(max_distance > diff){
                        max_distance = diff;
                        Selected_idx = i;
                    }
                }
                //visualize selected pose by glowing panel
                pose_cursor.SetActive(true);
                // move panel position
                pose_cursor.transform.position = PoseButtons[Selected_idx].transform.position;
            // confirm selected pose by lower all finger in to fist
            } else if (PointerPosition.y <= -0.3f && start_hover){
                // visualize circular progress and change the pose
                
                if (CircularProgress(0.5f)){
                    //set pose
                    PoseTemp.SetActive(false);
                    PoseTemp = PoseCollection[Selected_idx];
                    PoseTemp.SetActive(true);
                    pose_cursor.GetComponent<Image>().color = new Color32(240, 190, 0, 200);
                    menuActionController.ActionMode = "idle";
                    pose_menu_bg.GetComponent<Image>().color = new Color32(255, 255, 225, 100);
                    start_hover = false;
                }
            }else{
                _progress.value = 0;
                count_pause = 0f;
            }          
        }
    }
}
