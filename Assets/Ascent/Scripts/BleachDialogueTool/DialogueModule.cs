using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueIteration
{
    [System.Serializable]
    public class Dialogue
    {
        public string Text;
        [Tooltip("Only fill if needed, will default to voice babble clips.")]
        public AudioClip SpecificClipToPlay = null;
    }
    public List<Dialogue> DialogueText;

    [Tooltip("If the dialogue is a bit long winded check this to skip after first time to the next iteration element which could be the same gist written more concisely.")]
    public bool AutoIterateToNextDialogue = false;

    public bool PlayVoiceBabbleClips = true;
}

public class DialogueModule : MonoBehaviour
{
    [Header("Note: trigger must be Capseule collider")]
    [Header("0th elements should be left empty")]
    [SerializeField]
    [Tooltip("0th item should be left blank because of broken inspector element.")]
    private List<DialogueIteration> DialogueIterations;

    private int dialogueItrIndexToUse = 1;

    private CapsuleCollider dialogueTrigger;
    private TextWriter textWriter;
    private Tutorial tut;

    private bool isInZone = false;

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
        // Get the trigger collider on the object.
        foreach (CapsuleCollider sphCol in GetComponents<CapsuleCollider>())
        {
            if (sphCol.isTrigger)
            {
                dialogueTrigger = sphCol;
            }
        }

        textWriter = FindObjectOfType<TextWriter>();
        tut = FindObjectOfType<Tutorial>();
        Controls.Pilgrim.Interact.performed += ctx => CallWriter();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInZone = true;
            if (tut != null)
            {
                tut.ShowInteractionTutorial();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInZone = false;
            if (textWriter.InteractionStarted)
            {
                FinishInteraction();
                textWriter.ClearTextDisplay();
            }
            if (tut != null)
            {
                tut.ShowInteractionTutorial(false);
            }
        }
    }

    private void CallWriter()
    {
        if (isInZone)
        {
            if (!textWriter.InteractionStarted)
            {
                dialogueTrigger.radius *= 10;
            }
            textWriter.StartDisplayText(DialogueIterations[dialogueItrIndexToUse], this);
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

    public void FinishInteraction()
    {
        dialogueTrigger.radius /= 10;
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
