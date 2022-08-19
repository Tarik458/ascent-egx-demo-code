using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueIteration
{
    public List<string> Dialogue;
    [SerializeField]
    [Tooltip("If the dialogue is a bit long winded check this to skip after first time to the next iteration element which could be the same gist written more concisely.")]
    public bool AutoIterateToNextDialogue = false;
}

public class DialogueModule : MonoBehaviour
{

    [Header("0th element should be left empty")]
    [SerializeField]
    [Tooltip("0th item should be left blank because of broken inspector element.")]
    private List<DialogueIteration> DialogueIterations;

    private int dialogueItrIndexToUse = 1;

    private TextWriter textWriter;

    private bool isInZone = false;
    private bool canInteract = false;

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
        textWriter = FindObjectOfType<TextWriter>();
        Controls.Pilgrim.Interact.performed += ctx => CallWriter();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = true;
            isInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInZone = false;
            if (textWriter.InteractionStarted == false)
            {
                canInteract = false;
            }
        }
    }

    private void CallWriter()
    {
        if (isInZone || canInteract)
        {
            if (DialogueIterations[dialogueItrIndexToUse].AutoIterateToNextDialogue)
            {
                textWriter.StartDisplayText(DialogueIterations[dialogueItrIndexToUse], this, true);
            }
            else
            {
                textWriter.StartDisplayText(DialogueIterations[dialogueItrIndexToUse], this);
            }
        }
    }

    public void SetDialogueIterationToUse(int _dialogueIterationIndex)
    {
        dialogueItrIndexToUse = _dialogueIterationIndex;
    }

    public void IncrementDialogueIteration()
    {
        dialogueItrIndexToUse++;
    }

    public void EndInteraction()
    {
        canInteract = false;
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
