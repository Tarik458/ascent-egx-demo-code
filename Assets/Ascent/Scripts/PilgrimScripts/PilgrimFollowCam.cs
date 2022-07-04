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

    private Vector3 defaultCamOffset;
    private Quaternion defaultCamRotation;

    private bool offsetIsBusy = false;
    private bool rotIsBusy = false;
    private bool returnToDefaultOffset = false;
    private bool returnToDefaultRot = false;

    private void Start()
    {
        // Get initial camera offset.
        camOffset = transform.position - Target.position;
        defaultCamOffset = camOffset;
        defaultCamRotation = transform.rotation;
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
    /// <param name="_isExit"></param>
    public void AddOffset(Vector4 _vectorToAdd, bool _isExit = false)
    {
        // Call a coroutine to smoothly lerp cam offset. W value is desired duration.
        if (_isExit)
        {
            returnToDefaultOffset = true;
            if (offsetIsBusy)
            {
                offsetIsBusy = false;
            }
            StartCoroutine(LerpToDefaultOffset(_vectorToAdd, _vectorToAdd.w));
        }
        else
        {
            offsetIsBusy = true;
            if (returnToDefaultOffset)
            {
                returnToDefaultOffset = false;
            }
            StartCoroutine(LerpOffset(_vectorToAdd, _vectorToAdd.w));
        }
    }

    /// <summary>
    /// Define desired angle for the camera to end up facing.
    /// </summary>
    /// <param name="_angleToFace"></param>
    /// <param name="_isExit"></param>
    public void SetAngleToFace(Vector4 _angleToFace, bool _isExit = false)
    {
        // Call a coroutine to smoothly lerp angle cam is facing. W value is desired duration.
        if (_isExit)
        {
            returnToDefaultRot = true;
            if (rotIsBusy)
            {
                rotIsBusy = false;
            }
            StartCoroutine(LerpAngleToDefault(_angleToFace, _angleToFace.w));
        }
        else
        {
            rotIsBusy = true;
            if (returnToDefaultRot)
            {
                returnToDefaultRot = false;
            }
            StartCoroutine(LerpAngle(_angleToFace, _angleToFace.w));
        }
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
        Vector3 endPos = camOffset + _offsetAddition;

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
    }

    /// <summary>
    /// Coroutine to smoothly lerp cam offset to default value. W value is desired duration.
    /// </summary>
    /// <param name="_offsetAddition"></param>
    /// <param name="_lerpDuration"></param>
    /// <returns></returns>
    private IEnumerator LerpToDefaultOffset(Vector3 _offsetAddition, float _lerpDuration)
    {
        float elapsedTime = 0f;
        Vector3 startPos = camOffset;
        Vector3 endPos = defaultCamOffset;

        while (elapsedTime <= _lerpDuration)
        {
            if (returnToDefaultOffset == false)
            {
                yield break;
            }
            camOffset = Vector3.Lerp(startPos, endPos, elapsedTime / _lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        returnToDefaultOffset = false;
    }

    /// <summary>
    /// Coroutine to smoothly lerp angle cam is facing. W value is desired duration. isExit = true defines the angle should return to default.
    /// </summary>
    /// <param name="_angles"></param>
    /// <param name="_lerpDuration"></param>
    /// <param name="_isExit"></param>
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
        }
        rotIsBusy = false;
    }

    private IEnumerator LerpAngleToDefault(Vector3 _angles, float _lerpDuration)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = defaultCamRotation;

        while (elapsedTime <= _lerpDuration)
        {
            if (returnToDefaultRot == false)
            {
                yield break;
            }
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / _lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        returnToDefaultRot = false;
    }
}
