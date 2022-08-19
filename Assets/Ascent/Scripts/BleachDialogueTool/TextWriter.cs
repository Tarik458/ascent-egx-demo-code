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
    private TextMeshProUGUI TextBox;

    private int textIterator = 0;

    private bool isRunning = false;

    private bool isSpedUp = false;

    private void Start()
    {

        UIObjToShow.SetActive(false);
    }


    public void StartDisplayText(DialogueIteration _dialogueItrToUse, DialogueModule _caller, bool _autoIterate = false)
    {
        InteractionStarted = true;
        if (!isRunning)
        {

            UIObjToShow.SetActive(true);
            isRunning = true;
            StartCoroutine(DisplayText(textIterator, _dialogueItrToUse, _caller, _autoIterate));
        }
        else
        {
            isSpedUp = true;
        }
    }

    private IEnumerator DisplayText(int _dialogueIndex, DialogueIteration _dialogueItrToUse, DialogueModule _caller, bool _autoIterate)
    {
        string strToDisplay;
        AudioSource.Stop();
        if (textIterator < _dialogueItrToUse.Dialogue.Count)
        {
            for (int i = 0; i <= _dialogueItrToUse.Dialogue[_dialogueIndex].Length; i++)
            {
                strToDisplay = _dialogueItrToUse.Dialogue[_dialogueIndex].Substring(0, i);
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
        else if (textIterator == _dialogueItrToUse.Dialogue.Count && _autoIterate)
        {
            _caller.IncrementDialogueIteration();
            FinishDisplaying(_caller);
        }
        else if (textIterator == _dialogueItrToUse.Dialogue.Count)
        {
            FinishDisplaying(_caller);
        }
        isRunning = false;
        isSpedUp = false;
    }

    private void FinishDisplaying(DialogueModule _caller)
    {
        TextBox.text = string.Empty;
        _caller.EndInteraction();
        InteractionStarted = false;
        textIterator = 0;
        UIObjToShow.SetActive(false);
    }

}
