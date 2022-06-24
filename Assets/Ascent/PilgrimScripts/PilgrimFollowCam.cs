using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilgrimFollowCam : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The target for the camera to follow (the pilgrim)")]
    private Transform Target;

    [SerializeField]
    [Tooltip("The speed at which the camera will react to the target's movement, default 1")]
    private float FollowSpeed = 1f;

    /// <summary>
    /// Camera position in relation to the target object. Should be updated any time the camera position is changed.
    /// </summary>
    private Vector3 camOffset;

    private void Start()
    {
        // Get initial camera offset.
        camOffset = transform.position - Target.position;
    }

    private void LateUpdate()
    {
        Vector3 camDesiredPos = Target.position + camOffset;
        transform.position = Vector3.Lerp(transform.position, camDesiredPos, FollowSpeed * Time.deltaTime);
    }
}
