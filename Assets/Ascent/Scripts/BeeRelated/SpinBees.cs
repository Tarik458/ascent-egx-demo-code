using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBees : MonoBehaviour
{
    [SerializeField]
    private float speed = -75f;

    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime, 0f);
    }
}
