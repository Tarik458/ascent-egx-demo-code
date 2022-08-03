using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightenArea : MonoBehaviour
{
    [SerializeField]
    private List<FireFlicker> AreaFires;

    [SerializeField]
    private List<Light> AreaLights;

    [SerializeField]
    private float MaxIntensity;

    private float currentIntensity;

    private float intensityStep;

    private int litFires;

    private void Start()
    {
        intensityStep = MaxIntensity / AreaFires.Count;

        StartCoroutine(CheckFiresLitState());
    }

    private void FixedUpdate()
    {
        currentIntensity = Mathf.Lerp(currentIntensity, intensityStep * litFires, 0.01f);
        foreach (Light light in AreaLights)
        {
            light.intensity = currentIntensity;
        }
    }

    private IEnumerator CheckFiresLitState()
    {
        litFires = 0;
        foreach (FireFlicker fire in AreaFires)
        {
            if (fire.GetLitState())
            {
                litFires++;
            }
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CheckFiresLitState());
    }

}
