using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles trigger zones except crouch and camera affecting ones.
/// </summary>
public class TriggerZoneInfo : MonoBehaviour
{
    [HideInInspector]
    public bool inFireZone = false;
    [HideInInspector]
    public GameObject fireZoneObj;

    [HideInInspector]
    public bool inBeeZone = false;
    [HideInInspector]
    public bool beesFollowing = false;
    [HideInInspector]
    public GameObject beeZoneObj;

     
    public void TestInterations()
    {
        if (inFireZone)
        {
            fireZoneObj.GetComponent<FireFlicker>().LightFire();
        }
        if (inBeeZone && !beesFollowing)
        {
            beeZoneObj.GetComponent<Beeeeez>().SetTarget(this.gameObject.transform);
            beesFollowing = true;
        }
        else if (beesFollowing)
        {
            beeZoneObj.GetComponent<Beeeeez>().StopFollowing();
            beesFollowing = false;
        }
    }

    public void EnterFireZone(GameObject _objectRef)
    {
        inFireZone = true;
        fireZoneObj = _objectRef;
    }

    public void EnterBeeZone(GameObject _objectRef)
    {
        inBeeZone = true;
        beeZoneObj = _objectRef;
    }

}
