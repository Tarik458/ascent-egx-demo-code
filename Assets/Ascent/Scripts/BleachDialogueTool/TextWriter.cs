using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour
{
    [HideInInspector]
    public bool InteractionStarted = false;

    [SerializeField]
    [Tooltip("Object with the text box and any game overlay stuff to show/ hide.")]
    private GameObject UIObjToShow;

    [SerializeField]
    private List<AudioClip> VoiceBabbleClips;
    [SerializeField]
    private AudioSource AudioSource;

    [SerializeField]
    private float TextDisplaySpeed = 0.02f;
    [SerializeField]
    private float TextDisplaySpedUp = 0.005f;

    [SerializeField]
    private TextMeshProUGUI NameTextBox;

    [SerializeField]
    private TextMeshProUGUI TextBox;

    private int textIterator = 1;

    private bool isRunning = false;

    private bool isSpedUp = false;

    private void Start()
    {

        UIObjToShow.SetActive(false);
    }


    public void StartDisplayText(DialogueIteration _dialogueItrToUse, DialogueModule _caller)
    {
        InteractionStarted = true;
        if (!isRunning)
        {
            UIObjToShow.SetActive(true);
            isRunning = true;
            StartCoroutine(DisplayText(textIterator, _dialogueItrToUse, _caller));
        }
        else
        {
            isSpedUp = true;
        }
    }

    public void ClearTextDisplay()
    {
        FinishDisplaying();
    }

    private IEnumerator DisplayText(int _dialogueIndex, DialogueIteration _dialogueItrToUse, DialogueModule _caller)
    {
        string strToDisplay;
        AudioSource.Stop();
        if (textIterator < _dialogueItrToUse.DialogueText.Count)
        {
            if (_dialogueItrToUse.DialogueText[_dialogueIndex].Name != "")
            {
                NameTextBox.text = _dialogueItrToUse.DialogueText[_dialogueIndex].Name;
            }
            for (int i = 0; i <= _dialogueItrToUse.DialogueText[_dialogueIndex].Text.Length; i++)
            {
                strToDisplay = _dialogueItrToUse.DialogueText[_dialogueIndex].Text.Substring(0, i);
                TextBox.text = strToDisplay;
                // Play voice babble clips
                if(!AudioSource.isPlaying && _dialogueItrToUse.DialogueText[_dialogueIndex].SpecificClipToPlay == null && _dialogueItrToUse.PlayVoiceBabbleClips)
                {
                    AudioSource.PlayOneShot(VoiceBabbleClips[Random.Range(0, VoiceBabbleClips.Count)]);
                }
                else if (!AudioSource.isPlaying && _dialogueItrToUse.DialogueText[_dialogueIndex].SpecificClipToPlay != null)
                {
                    AudioSource.PlayOneShot(_dialogueItrToUse.DialogueText[_dialogueIndex].SpecificClipToPlay);
                }

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
        else if (textIterator == _dialogueItrToUse.DialogueText.Count && _dialogueItrToUse.AutoIterateToNextDialogue)
        {
            _caller.IncrementDialogueIteration();
            _caller.FinishInteraction();
            FinishDisplaying();
        }
        else if (textIterator == _dialogueItrToUse.DialogueText.Count)
        {
            _caller.FinishInteraction();
            FinishDisplaying();
        }
        isRunning = false;
        isSpedUp = false;
    }

    private void FinishDisplaying()
    {
        TextBox.text = string.Empty;
        InteractionStarted = false;
        textIterator = 1;
        UIObjToShow.SetActive(false);
    }

}
