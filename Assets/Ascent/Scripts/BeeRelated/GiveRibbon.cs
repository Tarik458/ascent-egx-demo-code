using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveRibbon : MonoBehaviour
{
    // Mount on Ill NPC object.
    private bool canGiveRibbon = false;

    /// <summary>
    /// Use Controls not _controls
    /// </summary>
    private PlayerControls _controls;
    /// <summary>
    /// Use Controls not _controls
    /// </summary>
    private PlayerControls Controls
    {
        get
        {
            if (_controls != null)
            {
                return _controls;
            }
            return _controls = new PlayerControls();
        }
    }

    void Start()
    {
        Controls.Pilgrim.Interact.performed += ctx => CheckGiveRibbon();
    }

    private void CheckGiveRibbon()
    {
        if (canGiveRibbon)
        {
            FindObjectOfType<MeadController>().RibbonsEnabled = true;
            this.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canGiveRibbon = true;
        }
    }

    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
