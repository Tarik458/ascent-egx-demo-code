using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlicker : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Point light on the torch / fire that wants flicker effect")]
    private Light AffectedLight;

    [SerializeField]
    [Tooltip("How fast the light flickers, could depend on wind etc, default 0.1f")]
    [Range(0.01f, 0.25f)]
    private float FlickerSpeed = 0.1f;

    [SerializeField]
    [Tooltip("Minimum brightness the flame will dip to, default 0.7f")]
    [Range(0.5f, 0.9f)]
    private float MinBrightness = 0.7f;

    [SerializeField]
    [Tooltip("Maximum brightness the flame will dip to, default 1.5f")]
    [Range(1.1f, 1.9f)]
    private float MaxBrightness = 1.5f;

    void FixedUpdate()
    {
        AffectedLight.intensity = Mathf.Lerp(AffectedLight.intensity, Random.Range(MinBrightness, MaxBrightness), FlickerSpeed);
    }
}
