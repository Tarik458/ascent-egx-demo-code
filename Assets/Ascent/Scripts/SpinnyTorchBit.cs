using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyTorchBit : MonoBehaviour
{

    private float bobRange = 0.25f;
    private Vector3 startPos;
    private Vector3 tmpPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        transform.Rotate(0f, 75 * Time.deltaTime, 0f);


        tmpPos = startPos;
        tmpPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 1) * 0.015f;

        transform.localPosition = tmpPos;
    }
}
