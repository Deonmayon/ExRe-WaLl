using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstruction : MonoBehaviour
{
    // Start is called before the first frame update
    bool ishit = false;
    int speedup;
    void Start()
    {
        speedup = 5;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (speedup+ HealthHeartManager.score) * -1 * Vector3.forward * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Freezepoint"))
        {
            // froze the menu after pase certain point
            menuActionController.ActionMode = "freeze";
            // speed up the wall
            speedup *= 2;
        } else if (other.gameObject.CompareTag("Endpoint") && !WallSpawner.is_create)
        {
            // de froze menu
            menuActionController.ActionMode = "idle";
            // destroy privious pose prefab
            Destroy(PoseMenuController.PoseTemp, .5f);
            // turn on idle pose
            PoseMenuController.pose_idle.SetActive(true);
            // OnTriggerEnter wall spawning
            WallSpawner.is_create = true;
            // deal damage
            if (ishit)
            {
                HealthHeartManager.health--;
            }
            else
            {
                HealthHeartManager.score++;
            }
        } else if (other.gameObject.CompareTag("pose")){
            // put damage flag to true
            ishit = true;
            // calculate damage in heath manager
        }
    }
}
