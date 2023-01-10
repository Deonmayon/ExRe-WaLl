using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragControl : MonoBehaviour
{
    [SerializeField] Collider thisCollider;
    Vector3 mousePosition, worldPosition;
    // Start is called before the first frame update
    Vector3 GetMousePosition(){
        // this will return position of camera that you are controlling
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    public void OnSelected(){
        // get pose of the selected object
        // render changing sprite
        GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
    }


    #region control by mouse
    void OnMouseDown()
    {
        // update relative position of the mouse to the POV
        //  (current mouse in world - pos of obj in camera POV = relative position to camera)
        mousePosition = Input.mousePosition - GetMousePosition();
    }

    void OnMouseDrag()
    {
         if (menuActionController.ActionMode == "move"){
            // get selected pose from system
            
            // transform position from screen to world
            worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            worldPosition = new Vector3(worldPosition.x, Mathf.Max(worldPosition.y,0), worldPosition.z);
            transform.position = worldPosition;
        }
    }
    #endregion
}
