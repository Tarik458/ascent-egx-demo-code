using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAdjustVals : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The vector to be added to the current camera offset from the player, W value is duration of movement, defaults to 1. eg. 10 in y field to move camera up by 10.")]
    private Vector4 DesiredTranslate;

    [SerializeField]
    [Tooltip("The x, y, z components to tell the camera which angle to face. W value is duration of movement, defaults to 1. Not an addition to current angle.")]
    private Vector4 DesiredRotation;

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

        return DesiredTranslate;
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

        return DesiredRotation;
    }

}
