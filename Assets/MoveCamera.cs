using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform endMarker = null; // create an empty gameobject and assign in inspector
    public bool moveCamera = false;
    
    public void triggerCamMovement()
    {
        moveCamera = true;
    }
    void Update()
    {
        if (moveCamera == true)
        {
            transform.position = Vector3.Lerp(transform.position, endMarker.position, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, endMarker.rotation, Time.deltaTime);
        }
       
    }
}
