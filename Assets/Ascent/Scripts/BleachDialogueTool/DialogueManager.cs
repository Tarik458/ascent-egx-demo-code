using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueIteration
    {
        public List<string> Dialogue;
    }


    [SerializeField]
    private List<AudioClip> VoiceBabbleClips;
    [SerializeField]
    private AudioSource AudioSource;

    [SerializeField]
    private float TextDisplaySpeed = 0.2f;
    [SerializeField]
    private float TextDisplaySpedUp = 0.005f;

    [SerializeField]
    [Tooltip("0th item should be left blank because of broken inspector element.")]
    private List<DialogueIteration> DialogueIterations;
    [SerializeField]
    [Tooltip("If the dialogue is a bit long winded check this to skip after first time to the second iteration element which could be the same gist written more concisely.")]
    private bool AutoIterateToSecond = false;

    [SerializeField]
    private TextMeshProUGUI TextBox;

    private int dialogueItrIndexToUse = 1;

    private int textIterator = 0;

    private bool isRunning = false;
    private bool isSpedUp = false;


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
        Controls.Pilgrim.Interact.performed += ctx => StartDisplayText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetDialogueIterationToUse(int _dialogueIterationIndex)
    {
        dialogueItrIndexToUse = _dialogueIterationIndex;
    }

    private void StartDisplayText()
    {
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(DisplayText(textIterator));
        }
        else
        {
            isSpedUp = true;
        }
    }

    private IEnumerator DisplayText(int _dialogueIndex)
    {
        string strToDisplay;
        AudioSource.Stop();
        if (textIterator < DialogueIterations[dialogueItrIndexToUse].Dialogue.Count)
        {
            for (int i = 0; i <= DialogueIterations[dialogueItrIndexToUse].Dialogue[_dialogueIndex].Length; i++)
            {
                strToDisplay = DialogueIterations[dialogueItrIndexToUse].Dialogue[_dialogueIndex].Substring(0, i);
                TextBox.text = strToDisplay;
                if (isSpedUp)
                {
                    yield return new WaitForSeconds(TextDisplaySpedUp);
                }
                else
                {
                    yield return new WaitForSeconds(TextDisplaySpeed);
                }
            }

        
            textIterator++;
        }
        // If it was the first time displaying dialogue switch to second dialogue block
        else if (textIterator == DialogueIterations[dialogueItrIndexToUse].Dialogue.Count && dialogueItrIndexToUse == 1 && AutoIterateToSecond)
        {
            TextBox.text = string.Empty;
            dialogueItrIndexToUse++;
            textIterator = 0;
        }
        else if(textIterator == DialogueIterations[dialogueItrIndexToUse].Dialogue.Count)
        {
            TextBox.text = string.Empty;
            textIterator = 0;
        }
        isRunning = false;
        isSpedUp = false;
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
