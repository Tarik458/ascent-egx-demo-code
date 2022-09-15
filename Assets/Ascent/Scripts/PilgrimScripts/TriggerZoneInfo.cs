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
    public bool InWater = false;
    [HideInInspector]
    public bool IsSlipping = false;


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
        if (!fireZoneObj.GetComponent<FireFlicker>().GetLitState() && tut != null)
        {
            tut.ShowInteractionTutorial();
        }
    }
    public void ExitFireZone()
    {
        inFireZone = false;
        if (tut != null)
        {
            tut.ShowInteractionTutorial(false);
        }
    }



    public void EnterBeeZone(GameObject _objectRef)
    {
        inBeeZone = true;
        beeZoneObj = _objectRef;
        beeeeez = beeZoneObj.GetComponent<Beeeeez>();
        if (tut != null)
        {
            tut.ShowInteractionTutorial();
        }
    }
    public void ExitBeeZone()
    {
        inBeeZone = false;
        if (tut != null)
        {
            tut.ShowInteractionTutorial(false);
        }
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
        if (!hiveZoneObj.GetComponent<HiveData>().GetRibbonState() && canPlaceRibbons && tut != null)
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
    public void ExitHiveZone()
    {
        inHiveZone = false;
        if (tut != null)
        {
            tut.ShowInteractionTutorial(false);
        }
    }

    public void ThrowIntoWater()
    {
        StartCoroutine(SlipSideways());
    }

    private IEnumerator SlipSideways()
    {
        IsSlipping = true;
        float timePassed = 0f;
        yield return new WaitForSeconds(0.5f);
        while (timePassed < 0.5f)
        {
            transform.Translate(new Vector3(-1.5f, 0f, 0.5f) * 5f * Time.deltaTime);
            timePassed += Time.deltaTime;
            yield return null;
        }
        IsSlipping = false;
    }

    public void FallIntoWater(Transform _visualComponent)
    {
            StartCoroutine(SwimToShore(_visualComponent));
    }

    private IEnumerator SwimToShore(Transform _visualComponent)
    {
        InWater = true;
        GetComponent<MixamoController>().EnterWater();
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Vector3 swimTarget = GameObject.Find("SwimTarget").transform.position;
        while (InWater)
        {
            transform.position = Vector3.MoveTowards(transform.position, swimTarget, 2f * Time.deltaTime);
            _visualComponent.rotation = Quaternion.RotateTowards(_visualComponent.rotation, Quaternion.LookRotation(swimTarget - transform.position).normalized, 720f * Time.deltaTime);
            if (transform.position == swimTarget)
            {
                InWater = false;
            }
            yield return null;
        }
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<MixamoController>().ExitWater();
    }

    public void EndGame(GameObject _objectRef)
    {
        _objectRef.GetComponent<ShowEndscreen>().ShowCanvas();
    }
}
