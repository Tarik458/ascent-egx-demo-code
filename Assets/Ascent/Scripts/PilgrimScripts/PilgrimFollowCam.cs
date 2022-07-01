using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilgrimFollowCam : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speed at which the camera will react to the target's movement, default 0.025f")]
    private float FollowSpeed = 0.025f;

    /// <summary>
    /// The target for the camera to follow (the pilgrim).
    /// </summary>
    private Transform Target;

    /// <summary>
    /// Camera position in relation to the target object. Should be updated any time the desired camera position is changed.
    /// </summary>
    private Vector3 camOffset;

    private void Start()
    {
        // Get initial camera offset.
        camOffset = transform.position - Target.position;
    }

    private void FixedUpdate()
    {
        // Using FixedUpdate and no time.deltatime solved camera jitter issue.
        Vector3 camDesiredPos = Target.position + camOffset;
        transform.position = Vector3.Lerp(transform.position, camDesiredPos, FollowSpeed);
    }

    /// <summary>
    /// Should be called by Pilgrim on start.
    /// </summary>
    /// <param name="_target"></param>
    public void SetTarget (Transform _target)
    {
        Target = _target;

        // Get initial camera offset.
        camOffset = transform.position - Target.position;
    }

    /// <summary>
    /// Add vector to current camera offset to change cam position in relation to target.
    /// </summary>
    /// <param name="_vectorToAdd"></param>
    public void AddOffset(Vector4 _vectorToAdd)
    {
        // Call a coroutine !!STARTCOROUTINE!! to smoothly lerp cam offset. W value is desired duration.
    }

    /// <summary>
    /// Define desired angle for the camera to end up facing.
    /// </summary>
    /// <param name="_angleToFace"></param>
    public void SetAngleToFace(Vector4 _angleToFace)
    {

    }

}
