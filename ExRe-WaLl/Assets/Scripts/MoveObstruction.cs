using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstruction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += -1*Vector3.forward * Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Freezepoint"))
        {
            // froze the menu after pase certain point
        } else if (other.gameObject.CompareTag("Endpoint")){
            // destroy the wall after surpass entire point
        } else if (other.gameObject.CompareTag("pose")){
            // put damage flag to true
            // calculate damage in heath manager
        }
    }
}
