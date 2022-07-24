using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beeeeez : MonoBehaviour
{
    private Transform Target;

    private Vector3 beeOffset;

    private float followSpeed = 0.05f;

    private bool isFollowing = false;

    private void SetOffset()
    {
        beeOffset = transform.position - Target.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isFollowing)
        {
            Vector3 beeDesiredPos = Target.position + beeOffset;
            transform.position = Vector3.Lerp(transform.position, beeDesiredPos, followSpeed);
        }
    }


    public void SetTarget(Transform _target)
    {
        Target = _target;
        SetOffset();
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }
}
