
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
using System.Threading;
using System;
using System.Linq;
using System.Security.Cryptography;

public class WallSpawner : MonoBehaviour
{
    public OjectScriptable object_manager;
    GameObject target_obj, newobj;
    public static bool is_create;
    public static int rand_idx_wall;
    [SerializeField] GameObject Door;
    private Animator DoorAnimator;
    void Start()
    {
        is_create = true;
        DoorAnimator = Door.GetComponent<Animator>();
    }

    bool is_in(int[] arr, int target){
        for(int i = 0; i < arr.Length; i++){
            if(target == arr[i]){
                return true;
            }
        }
        return false;
    }

    public void Shuffle(int[] arr) 
    {
        int tempGO;
        for (int i = 0; i < arr.Length - 1; i++) 
        {
            int rnd = Random.Range(i, arr.Length);
            tempGO = arr[rnd];
            arr[rnd] = arr[i];
            arr[i] = tempGO;
        }
    }

    void Update()
    {
        if (is_create){
            // destroy previous wall
            Destroy(newobj);
             // spawn 1 walls at the time
            // random select 1 wall from object manager
            rand_idx_wall = Random.Range(0,object_manager.WallsSprites.Length);
            target_obj = object_manager.WallsSprites[rand_idx_wall];
            
            // reset target wall stage
            for(int i = 0; i < target_obj.transform.childCount; i++){
                target_obj.transform.GetChild(i).gameObject.SetActive(false);
            }
            // random size of the wall
            target_obj.transform.GetChild(Random.Range(0, target_obj.transform.childCount)).gameObject.SetActive(true);

            // do the wall animation
            DoorAnimator.SetTrigger("TrOpen");
            // instant that prefab
            newobj = Instantiate(target_obj, this.transform);
            // create button for selecting pose
            PoseMenuController.button_skin[0] = rand_idx_wall;
            for (int i = 1; i < 3; i++){
                int rand_idx_button = Random.Range(0,object_manager.PosesSprites.Length);
                // re-index if it is already in the list
                while(is_in(PoseMenuController.button_skin, rand_idx_button)){
                    rand_idx_button = Random.Range(0,object_manager.PosesSprites.Length);
                } 
                // update pose
                PoseMenuController.button_skin[i] = rand_idx_button;
                Debug.LogFormat("{0}", PoseMenuController.button_skin);
            }
            // shuffle all button order
            Shuffle(PoseMenuController.button_skin);
            // update status to generate selected
            is_create = false;
        }
    }
}
