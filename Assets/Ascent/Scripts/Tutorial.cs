using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    public bool PlayTutorial = true;

    [SerializeField]
    private int NumberOfWASDNeeded = 4;


    [SerializeField]
    private GameObject WASDVisual;
    [SerializeField]
    private GameObject JumpVisual;
    [SerializeField]
    private GameObject InteractionVisual;



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

    public void ShowInteractionTutorial(bool _show = true)
    {
        if (PlayTutorial && tutorialIteration == 2)
        {
            InteractionVisual.SetActive(_show);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        WASDVisual.SetActive(false);
        JumpVisual.SetActive(false);
        InteractionVisual.SetActive(false);
        if (PlayTutorial)
        {
            Controls.Pilgrim.Movement.performed += CheckForWASD;
            DisplayTutorial();
        }
        Controls.Pilgrim.Interact.performed += ctx => ShowInteractionTutorial(false);
    }

    private void DisplayTutorial()
    {
        switch (tutorialIteration)
        {
            case 0:
                // WASD
                WASDVisual.SetActive(true);
                break;
            case 1:
                // Jump
                WASDVisual.SetActive(false);
                JumpVisual.SetActive(true);
                break;
            case 2:
                // Interact
                JumpVisual.SetActive(false);
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
        if (completionCounter == NumberOfWASDNeeded)
        {
            tutorialIteration++;
            DisplayTutorial();
            Controls.Pilgrim.Movement.performed -= CheckForWASD;
            Controls.Pilgrim.Jump.performed += CheckForJump;
        }

    }

    private void CheckForJump(InputAction.CallbackContext _ctx)
    {
        tutorialIteration++;
        DisplayTutorial();
        Controls.Pilgrim.Jump.performed -= CheckForJump;
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
