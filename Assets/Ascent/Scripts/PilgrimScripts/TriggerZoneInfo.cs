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


    // Bee Stuff
    [HideInInspector]
    public bool canControlBees = false;

    [HideInInspector]
    public bool inBeeZone = false;
    [HideInInspector]
    public bool beesFollowing = false;
    [HideInInspector]
    public GameObject beeZoneObj;
    private Beeeeez beeeeez;

    [HideInInspector]
    public bool canPlaceRibbons = false;
    [HideInInspector]
    public bool inHiveZone = false;
    [HideInInspector]
    public GameObject hiveZoneObj;

    [HideInInspector]
    public Tutorial tut;

    private void Start()
    {
        tut = FindObjectOfType<Tutorial>();
    }

    public void TestInterations(MixamoController _mixamo)
    {
        if (inFireZone)
        {
            if (fireZoneObj.GetComponent<FireFlicker>().GetLitState() == false)
            {
                fireZoneObj.GetComponent<FireFlicker>().LightFire();
                if (fireZoneObj.GetComponent<FireFlicker>().IsWallFireQuery())
                {
                    _mixamo.LightWallFire();
                }
                else 
                {
                    _mixamo.LightFire(); 
                }
            }
        }

        if (canControlBees)
        {
            if (inBeeZone && !beesFollowing)
            {
                beeeeez.SetTarget(this.gameObject.transform);
                beesFollowing = true;
            }
            else if (beesFollowing)
            {
                beeeeez.StopFollowing();
                beesFollowing = false;
                Debug.Log("bees stop following");
            }
        }

        if (inHiveZone && canPlaceRibbons)
        {
            hiveZoneObj.GetComponent<HiveData>().ApplyRibbon();
        }
    }

    public void EnterFireZone(GameObject _objectRef)
    {
        inFireZone = true;
        fireZoneObj = _objectRef;
        if (!fireZoneObj.GetComponent<FireFlicker>().GetLitState())
        {
            tut.ShowInteractionTutorial();
        }
    }

    public void EnterBeeZone(GameObject _objectRef)
    {
        inBeeZone = true;
        beeZoneObj = _objectRef;
        beeeeez = beeZoneObj.GetComponent<Beeeeez>();
        tut.ShowInteractionTutorial();
    }

    public void ScareBees()
    {
        beesFollowing = false;
        beeeeez.ScaredStopFollowing();
    }

    public bool EnterHiveZone(GameObject _objectRef)
    {
        inHiveZone = true;
        hiveZoneObj = _objectRef;
        if (!hiveZoneObj.GetComponent<HiveData>().GetRibbonState() && canPlaceRibbons)
        {
            tut.ShowInteractionTutorial();
        }
        if (beesFollowing)
        {
            if (_objectRef.GetComponent<HiveData>().BeesEnter(beeeeez))
            {
                beesFollowing = false;
            }
        }

        return _objectRef.GetComponent<HiveData>().GetRibbonState();
    }


    public void EndGame(GameObject _objectRef)
    {
        _objectRef.GetComponent<ShowEndscreen>().ShowCanvas();
    }
}
