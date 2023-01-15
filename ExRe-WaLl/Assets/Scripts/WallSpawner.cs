
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
    GameObject target_obj;
    public static bool is_create;
    public static int rand_idx_wall;
    void Start()
    {
        is_create = true;
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
             // spawn 1 walls at the time
            // random select 1 wall from object manager
            rand_idx_wall = Random.Range(0,object_manager.WallsSprites.Length);
            target_obj = object_manager.WallsSprites[rand_idx_wall];
            // do the wall animation

            // instant that prefab
            GameObject newobj = Instantiate(target_obj);
            // create button for selecting pose
            PoseMenuController.button_skin[0] = Random.Range(0,object_manager.PosesSprites.Length);
            for (int i = 1; i > object_manager.PosesSprites.Length; i++){
                int rand_idx_button = Random.Range(0,object_manager.PosesSprites.Length);
                // re-index if it is already in the list
                while(!is_in(PoseMenuController.button_skin, rand_idx_button)){
                    rand_idx_button = Random.Range(0,object_manager.PosesSprites.Length);
                } 
                // update pose
                PoseMenuController.button_skin[i] = rand_idx_button;
            }
            // shuffle all button order
            Shuffle(PoseMenuController.button_skin);
            // update status to generate selected
            is_create = false;
        }
    }
}
