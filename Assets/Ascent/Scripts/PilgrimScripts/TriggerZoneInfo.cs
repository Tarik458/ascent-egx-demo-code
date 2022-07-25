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
    private Beeeeez beeeeez;

    [HideInInspector]
    public bool inHiveZone = false;
    [HideInInspector]
    public GameObject hiveZoneObj;

    public void TestInterations(MixamoController _mixamo)
    {
        if (inFireZone)
        {
            if (fireZoneObj.GetComponent<FireFlicker>().GetLitState() == false)
            {
                fireZoneObj.GetComponent<FireFlicker>().LightFire();
                _mixamo.LightFire();
            }
        }

        if (inBeeZone && !beesFollowing)
        {
            beeeeez.SetTarget(this.gameObject.transform);
            beesFollowing = true;
        }
        else if (beesFollowing)
        {
            beeeeez.StopFollowing();
            beesFollowing = false;
        }

        if (inHiveZone)
        {
            hiveZoneObj.GetComponent<HiveData>().ApplyRibbon();
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
        beeeeez = beeZoneObj.GetComponent<Beeeeez>();
    }

    public void ScareBees()
    {
        beesFollowing = false;
        beeeeez.ScaredStopFollowing();
    }

    public bool EnterHiveZone(GameObject _objectRef)
    {
        if (beesFollowing)
        {
            _objectRef.GetComponent<HiveData>().BeesEnter(beeeeez);
            beesFollowing = false;
        }

        return _objectRef.GetComponent<HiveData>().GetRibbonState();
    }

}
