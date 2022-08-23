using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlicker : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Default is unlit, only torch should start lit")]
    protected bool IsLit = false;

    [SerializeField]
    [Tooltip("Point light on the torch / fire that wants flicker effect")]
    protected Light AffectedLight;

    [SerializeField]
    [Tooltip("How fast the light flickers, could depend on wind etc, default 0.1f")]
    [Range(0.01f, 0.25f)]
    protected float FlickerSpeed = 0.1f;

    [SerializeField]
    [Tooltip("Minimum brightness the flame will dip to, default 0.7f")]
    [Range(0f, 8f)]
    protected float MinBrightness = 0.7f;

    [SerializeField]
    [Tooltip("Maximum brightness the flame will dip to, default 1.5f")]
    [Range(0f, 25f)]
    protected float MaxBrightness = 1.5f;

    [SerializeField]
    [Tooltip("Particle effect to play when fire gets lit.")]
    protected ParticleSystem IgniteFireParticle = null;

    [SerializeField]
    [Tooltip("Particle effect to play when fire is lit.")]
    protected ParticleSystem FireParticle;

    [SerializeField]
    [Tooltip("Optional Particle effect to play when fire gets lit.")]
    protected ParticleSystem GodRayParticle = null;

    [SerializeField]
    protected AudioClip lightUpFire;

    [SerializeField]
    AudioSource audioSource;


    protected bool isFlickering;

    public bool GetLitState()
    {
        return IsLit;
    }

    public void LightFire()
    {
        IsLit = true;
        Light(IsLit);
        StartCoroutine(Ignition());
        IgniteFireParticle.Play();
        audioSource.PlayOneShot(lightUpFire);
    }

    protected void Start()
    {
        Light(IsLit);
        isFlickering = IsLit;
    }

    protected void FixedUpdate()
    {
        if (IgniteFireParticle != null)
        {
            if (IsLit && !IgniteFireParticle.isPlaying && !FireParticle.isPlaying)
            {
                FireParticle.Play();
            }
        }
        if (isFlickering)
        {
            AffectedLight.intensity = Mathf.Lerp(AffectedLight.intensity, Random.Range(MinBrightness, MaxBrightness), FlickerSpeed);
        }
    }

    /// <summary>
    /// Increases the brightness of the flame as it is lit.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Ignition()
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
        isFlickering = true;
        if (GodRayParticle != null)
        {
            GodRayParticle.Play();
        }
        
    }

    /// <summary>
    /// Set the parent object of the light component active or inactive as defined by "status".
    /// </summary>
    /// <param name="status"></param>
    protected void Light(bool status)
    {
        AffectedLight.gameObject.SetActive(status);
    }

}
