using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseMenuController : MonoBehaviour
{
    public OjectScriptable object_manager;

    [SerializeField] GameObject[] PoseButtons;
    public static GameObject PoseTemp;
    public static GameObject pose_idle;
    [Space]
    [SerializeField] GameObject pose_menu_bg;
    [SerializeField] GameObject pose_cursor;
    [Space]
    [SerializeField] GameObject Progress_bar;
    Slider _progress;
    Vector3 PointerPosition;
    int Selected_idx;
    float[] index_values = new float[]{0.40f, 0f, -0.40f};
    public int[] button_skin_temp = new int[] { -1, -1, -1 };
    public static int[] button_skin = new int[]{-1,-1,-1};
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
        pose_idle = GameObject.Find("idle_pose");
        // PoseTemp.SetActive(true);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        #region generate button for pose
        // update button skin
        // PoseButtons for loop change
        if (!WallSpawner.is_create)
        {
            for (int i = 0; i < 3; i++)
            {
                //Debug.LogFormat("{0} : {1}", button_skin[i], object_manager.PosesButtons[button_skin[i]]);
                PoseButtons[i].GetComponent<RawImage>().texture = object_manager.PosesButtons[button_skin[i]];
            }
            button_skin_temp = button_skin;
        }
        // change on select buttion use the index from button_skin
        #endregion

        #region selecting pose
        // Debug.LogFormat("{0} {1} {2} {3} {4}",fingerups[0],fingerups[1],fingerups[2],fingerups[3],fingerups[4]);
        // if Action mode is Idle then we can select the pose from the nearest x value
        if (menuActionController.ActionMode == "select"){
            // get hand location
            PointerPosition = menuActionController.cursor_positions;
            // Debug.LogFormat("{0}",PointerPosition);
            // check y position if It is hovering at pose menu
            if (PointerPosition.y >= 0.25){
                // visualizing selected menu (may be glowing panel)
                pose_menu_bg.GetComponent<Image>().color = new Color32(0, 255, 180, 100);
                // reset in tokens
                max_distance = 999;
                //flag selected
                start_hover = true;
                //check the x position and determine which menu it is selecting
                
                for(int i=0; i < PoseButtons.Length; i++){
                    diff = Mathf.Abs(index_values[i] - PointerPosition.x);
                    //Debug.LogFormat("{0}", diff);
                    if (max_distance > diff){
                        max_distance = diff;
                        Selected_idx = i;
                    }
                }
                //visualize selected pose by glowing panel
                pose_cursor.SetActive(true);
                // move panel position
                pose_cursor.transform.position = PoseButtons[Selected_idx].transform.position;
            // confirm selected pose by lower all finger in to fist
            } else if (PointerPosition.y <= -0.1f && start_hover){
                // visualize circular progress and change the pose
                if (CircularProgress(0.5f)){
                    // destroy privious pose prefab
                    Destroy(PoseTemp, .5f);
                    // select pose // change on select buttion use the PoseTemp = object_manager.PosesSprites[button_skin[Selected_idx]]
                    PoseTemp = object_manager.PosesSprites[button_skin[Selected_idx]];
                    // PoseTemp = PoseCollection[Selected_idx];
                    PoseTemp = Instantiate(PoseTemp, this.gameObject.transform);
                    pose_cursor.GetComponent<Image>().color = new Color32(240, 190, 0, 200);
                    menuActionController.ActionMode = "idle";
                    pose_menu_bg.GetComponent<Image>().color = new Color32(255, 255, 225, 100);
                    start_hover = false;
                    _progress.value = 0;
                    // set idle temp off
                    pose_idle.SetActive(false);
                }
            }else{
                // _progress.value = 0;
                // count_pause = 0f;
            }          
        }
        #endregion
    }
}
