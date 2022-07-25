using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAdjustVals : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The vector to be added to the current camera offset from the player, W value is duration of movement, defaults to 1. eg. 10 in y field to move camera up by 10.")]
    private Vector4 DesiredTranslate;

    [SerializeField]
    [Tooltip("The x, y, z components to tell the camera which angle to face, can copy paste from test object transform rotation. " +
        "W value is duration of movement, defaults to 1. Not an addition to current angle.")]
    private Vector4 DesiredRotation;

    [SerializeField]
    [Tooltip("The initally enabled trigger zone. Should be further through the level than the Backtrack Trigger zone.")]
    private GameObject ForwardsTrigger;

    [SerializeField]
    [Tooltip("The trigger zone to reverse the effect on the camera that the Forwards Trigger(FT) zone has. Should be placed behind the FT zone in terms of level progression." +
        "Automatically disabled when script starts.")]
    private GameObject BacktrackTrigger;

    private bool isReversed = false;

    private Vector4 previousRotation;

    private void Start()
    {
        BacktrackTrigger.SetActive(false);
    }

    /// <summary>
    /// The value that entering the zone should add to the current camera offset.
    /// </summary>
    /// <returns></returns>
    public Vector4 GetAdditionToOffset()
    {
        if (DesiredTranslate.w == 0)
        {
            DesiredTranslate.w = 1f;
        }
        if (isReversed)
        {
            return DesiredTranslate * -1;
        }
        else
        {
            return DesiredTranslate;
        }
    }

    /// <summary>
    /// The desired angle for the camera to face, not an addition to current angle.
    /// </summary>
    /// <returns></returns>
    public Vector4 GetDesiredCamRotation()
    {
        if (DesiredRotation.w == 0)
        {
            DesiredRotation.w = 1f;
        }
        if (isReversed && previousRotation.w == 0)
        {
            previousRotation.w = 1f;
        }

        if (isReversed)
        {
            return previousRotation;
        }
        else
        {
            return DesiredRotation;
        }
    }

    /// <summary>
    /// Stores previous rotation of camera to use for backtracking.
    /// </summary>
    /// <param name="_prevRot"></param>
    public void SetPreviousRotation(Vector3 _prevRot)
    {
        previousRotation = new Vector4(_prevRot.x, _prevRot.y, _prevRot.z, DesiredRotation.w);
    }

    /// <summary>
    /// Disable currently active trigger zone and enable the inactive one. Also toggles isReversed bool.
    /// </summary>
    /// <returns></returns>
    public void ToggleEnabledZone()
    {
        ForwardsTrigger.SetActive(isReversed);
        isReversed = !isReversed;
        BacktrackTrigger.SetActive(isReversed);
    }
}
