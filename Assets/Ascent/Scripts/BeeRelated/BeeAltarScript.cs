using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAltarScript : FireFlicker
{
    private bool pilgrimBeePowerEnabled = false;

    private MeadController meadController;

    private void Start()
    {
        meadController = FindObjectOfType<MeadController>();
    }

    private void Update()
    {
        if (IsLit && !pilgrimBeePowerEnabled)
        {
            meadController.BeePowersEnabled = true;
            pilgrimBeePowerEnabled = true;
        }
    }
}
