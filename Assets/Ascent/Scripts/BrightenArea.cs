using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightenArea : MonoBehaviour
{
    [SerializeField]
    private List<FireFlicker> AreaFires;

    [SerializeField]
    private Light AreaLight;

    [SerializeField]
    private float MaxIntensity;


    private float startIntensity = 0f;
    private float currentIntensity;

    private float intensityStep;

    private int litFires;

    private void Start()
    {
        currentIntensity = startIntensity;

        intensityStep = MaxIntensity / AreaFires.Count;

        StartCoroutine(CheckFiresLitState());
    }

    private void Update()
    {
        
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
