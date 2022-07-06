using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilgrimFollowCam : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The speed at which the camera will react to the target's movement, default 0.025f")]
    private float FollowSpeed = 0.025f;

    [SerializeField]
    [Tooltip("Desired position of the camera in the world at the start of each level.")]
    private Vector3 StartCamPosition;
    [SerializeField]
    [Tooltip("If false will just use current camera position in scene.")]
    private bool UseStartCamPosition;
    [SerializeField]
    [Tooltip("Desired rotation of the camera at the start of each level.")]
    private Vector3 StartCamRotation;
    [SerializeField]
    [Tooltip("If false will just use current camera rotation in scene.")]
    private bool UseStartCamRotation;

    /// <summary>
    /// The target for the camera to follow (the pilgrim).
    /// </summary>
    private Transform Target;

    /// <summary>
    /// Camera position in relation to the target object. Should be updated any time the desired camera position is changed.
    /// </summary>
    private Vector3 camOffset;
    private Vector3 baseCamOffset;
    private Vector3 previousCamRotation;

    private bool offsetIsBusy = false;
    private bool rotIsBusy = false;

    private void Start()
    {
        // Set camera to the desired start position and rotation.
        if (UseStartCamPosition)
        {
            transform.position = StartCamPosition;
        }
        if (UseStartCamRotation)
        {
            transform.rotation = Quaternion.Euler(StartCamRotation);
        }
        // Get initial camera offset.
        baseCamOffset = transform.position - Target.position;
        camOffset = baseCamOffset;
        previousCamRotation = transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        // Using FixedUpdate and no time.deltatime solved camera jitter issue.
        Vector3 camDesiredPos = Target.position + camOffset;
        transform.position = Vector3.Lerp(transform.position, camDesiredPos, FollowSpeed);
    }

    public Vector3 GetPrevCamRot()
    {
        return previousCamRotation;
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
        // Call a coroutine to smoothly lerp cam offset. W value is desired duration.
        offsetIsBusy = false;
        offsetIsBusy = true;
        StartCoroutine(LerpOffset(_vectorToAdd, _vectorToAdd.w));
        
    }

    /// <summary>
    /// Define desired angle for the camera to end up facing.
    /// </summary>
    /// <param name="_angleToFace"></param>
    public void SetAngleToFace(Vector4 _angleToFace)
    {
        // Call a coroutine to smoothly lerp angle cam is facing. W value is desired duration.
        previousCamRotation = transform.eulerAngles;
        rotIsBusy = false;
        rotIsBusy = true;
        StartCoroutine(LerpAngle(_angleToFace, _angleToFace.w));
    }


    /// <summary>
    /// Coroutine to smoothly lerp cam offset. W value is desired duration.
    /// </summary>
    /// <param name="_offsetAddition"></param>
    /// <param name="_lerpDuration"></param>
    /// <returns></returns>
    private IEnumerator LerpOffset(Vector3 _offsetAddition, float _lerpDuration)
    {
        float elapsedTime = 0f;
        Vector3 startPos = camOffset;
        Vector3 endPos = baseCamOffset + _offsetAddition;

        if (_lerpDuration < 0)
        {
            _lerpDuration *= -1;
        }

        while (elapsedTime <= _lerpDuration)
        {
            if (offsetIsBusy == false)
            {
                yield break;
            }
            camOffset = Vector3.Lerp(startPos, endPos, elapsedTime / _lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        offsetIsBusy = false;
        baseCamOffset = endPos;
    }

    /// <summary>
    /// Coroutine to smoothly lerp angle cam is facing. W value is desired duration. isExit = true defines the angle should return to default.
    /// </summary>
    /// <param name="_angles"></param>
    /// <param name="_lerpDuration"></param>
    /// <returns></returns>
    private IEnumerator LerpAngle(Vector3 _angles, float _lerpDuration)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(_angles);

        while (elapsedTime <= _lerpDuration)
        {
            if (rotIsBusy == false)
            {
                yield break;
            }
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / _lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
            transform.rotation = endRotation;
        }
        rotIsBusy = false;
    }
}
