using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAdjustVals : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The vector to be added to the current camera offset from the player, eg. 10 in y field to move camera up by 10.")]
    private Vector3 DesiredTranslate;

    [SerializeField]
    [Tooltip("The x, y, z components to tell the camera which angle to face. Not an addition to current angle.")]
    private Vector3 DesiredRotation;

    /// <summary>
    /// The value that entering the zone should add to the current camera offset.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetAdditionToOffset()
    {
        return DesiredTranslate;
    }

    /// <summary>
    /// The desired angle for the camera to face, not an addition to current angle.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDesiredCamRotation()
    {
        return DesiredRotation;
    }

}
