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
    Vector3 PointerPosition;
    int Selected_idx;
    float[] index_values = new float[]{0.16f, 0.50f, 0.83f};
    float diff, max_distance = 1;

    bool IsFisting(Vector3[] positions){
        return true;
    }

    bool CircularProgress(float timer){
        return true;
    }
    void Start()
    {
        // PoseTemp = PoseCollection[0];
        // PoseTemp.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // if Action mode is Idle then we can select the pose from the nearest x value
        if (menuActionController.ActionMode == "select"){
            // get hand location
            PointerPosition = menuActionController.cursor_positions;
            // check y position if It is hovering at pose menu
            if (PointerPosition.y >= 0.8){
                // visualizing selected menu (may be glowing panel)
                pose_menu_bg.GetComponent<Image>().color = new Color32(0, 255, 180, 100);
                // reset in tokens
                max_distance = 1;
                //check the x position and determine which menu it is selecting
                for(int i=0; i < PoseCollection.Length; i++){
                    diff = Mathf.Abs(index_values[i] - PointerPosition.x);
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
            } else if (PointerPosition.y >= 0.6){
                // visualize circular progress and change the pose
                if (CircularProgress(0.5f)){
                    //set pose
                    PoseTemp.SetActive(false);
                    PoseTemp = PoseCollection[Selected_idx];
                    PoseTemp.SetActive(true);
                }
            } else{
                pose_menu_bg.GetComponent<Image>().color = new Color32(255, 255, 225, 100);
                pose_cursor.SetActive(false);
            }
        }
    }
}
