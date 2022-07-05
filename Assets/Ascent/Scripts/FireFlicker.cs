using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlicker : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Default is unlit, only torch should start lit")]
    private bool IsLit = false;

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

    [SerializeField]
    private AudioClip lightUpFire;

    [SerializeField]
    AudioSource audioSource;


    public void LightFire()
    {
        Light(true);
        StartCoroutine(Ignition());
        audioSource.PlayOneShot(lightUpFire);
    }

    private void Start()
    {
        Light(IsLit);
    }

    private void FixedUpdate()
    {
        if (IsLit)
        {
            AffectedLight.intensity = Mathf.Lerp(AffectedLight.intensity, Random.Range(MinBrightness, MaxBrightness), FlickerSpeed);
        }
    }

    /// <summary>
    /// Increases the brightness of the flame as it is lit.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Ignition()
    {
        float ignitionDuration = 2.5f;
        float elapsedTime = 0f;
        float initialBrightness = 0.1f;
        float targetBrightness = 1f;

        while (elapsedTime <= ignitionDuration)
        {
            AffectedLight.intensity = Mathf.Lerp(initialBrightness, targetBrightness, elapsedTime / ignitionDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        IsLit = true;
    }

    /// <summary>
    /// Set the parent object of the light component active or inactive as defined by "status".
    /// </summary>
    /// <param name="status"></param>
    private void Light(bool status)
    {
        AffectedLight.gameObject.SetActive(status);
    }

}
