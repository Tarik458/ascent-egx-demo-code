using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{



    private int tutorialIteration = 0;

    private bool[] WASDComplete = new bool[] {false, false, false, false};


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
    // Start is called before the first frame update
    void Start()
    {
        Controls.Pilgrim.Movement.performed += CheckForWASD;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayTutorial();    
    
    }

    private void DisplayTutorial()
    {
        switch (tutorialIteration)
        {
            case 0:
                // WASD

                break;
            case 1:
                // Jump

                break;
            case 2:
                // Interact

                break;
        }
    }

    private void CheckForWASD(InputAction.CallbackContext _ctx)
    {
        int completionCounter = 0;
        switch (_ctx.ReadValue<Vector2>().x)
        {
            case > 0:
                // D
                WASDComplete[3] = true;
                break;
            case < 0:
                // A 
                WASDComplete[1] = true;
                break;
        }
        switch (_ctx.ReadValue<Vector2>().y)
        {
            case > 0:
                // W
                WASDComplete[0] = true;
                break;
            case < 0:
                // S
                WASDComplete[2] = true;
                break;
        }
        foreach (bool b in WASDComplete)
        {
            if (b == true)
            {
                completionCounter++;
            }
        }
        if (completionCounter == 4)
        {
            tutorialIteration++;
            Debug.Log("Completed WASD tut");
            Controls.Pilgrim.Movement.performed -= CheckForWASD;
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
