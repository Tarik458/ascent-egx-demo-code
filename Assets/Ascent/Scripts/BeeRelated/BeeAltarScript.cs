using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeAltarScript : FireFlicker
{
    private bool pilgrimBeePowerEnabled = false;

    [SerializeField]
    private MeadController meadController;

    private void Update()
    {
        if (IsLit && !pilgrimBeePowerEnabled)
        {
            meadController.BeePowersEnabled = true;
            pilgrimBeePowerEnabled = true;
        }
    }
}
