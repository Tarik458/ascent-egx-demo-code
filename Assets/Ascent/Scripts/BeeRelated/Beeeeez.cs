using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beeeeez : MonoBehaviour
{
    [SerializeField]
    private int SwarmNumber;

    private Transform Target;

    private Vector3 idleTargetPos;

    private Vector3 beeOffset;

    private float followSpeed = 0.05f;

    private bool isFollowing = false;
    private bool isScared = false;

    private SwarmLocationData swarmLocationData;

    private void Start()
    {
        idleTargetPos = transform.position;
        swarmLocationData = GetComponentInParent<SwarmLocationData>();
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isFollowing)
        {
            Vector3 beeDesiredPos = Target.position + beeOffset;
            transform.position = Vector3.Lerp(transform.position, beeDesiredPos, followSpeed);
        }
        else if(isScared)
        { 
            // TODO:
            // needs some work, constant speed and when reach target set isScare = false
            transform.position = Vector3.Lerp(transform.position, idleTargetPos, followSpeed);
        }
    }

    public int GetSwarmNumber()
    {
        return SwarmNumber;
    }

    /// <summary>
    /// Sets the pilgrim as the target to follow and sets the swarm location to unoccupied.
    /// </summary>
    /// <param name="_target"></param>
    public void SetTarget(Transform _target)
    {
        isScared = false;
        Target = _target;
        SetOffset();
        isFollowing = true;
        swarmLocationData.SetLocationState(idleTargetPos, false);
    }

    public void EnterHive(Vector3 _hivePos)
    {
        isFollowing = false;
        StartCoroutine(TranslateToHive(_hivePos));
    }
    private IEnumerator TranslateToHive(Vector3 _desiredPos)
    {
        Vector3 startPos = transform.position;
        float timeToLerp = 1.5f;
        float elapsedTime = 0f;

        while (elapsedTime <= timeToLerp)
        {
            transform.position = Vector3.Lerp(startPos, _desiredPos, elapsedTime/timeToLerp);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }




    /// <summary>
    /// If user decides to leave bees where they are.
    /// </summary>
    public void StopFollowing()
    {
        isFollowing = false;
    }

    /// <summary>
    /// If bees are scared by environment, returns them to an idling positon.
    /// </summary>
    public void ScaredStopFollowing()
    {
        isFollowing = false;
        idleTargetPos = swarmLocationData.GetEmptyLocationAndSetOccupied();
        isScared = true;
    }



    private void SetOffset()
    {
        beeOffset = transform.position - Target.position;
    }
}
