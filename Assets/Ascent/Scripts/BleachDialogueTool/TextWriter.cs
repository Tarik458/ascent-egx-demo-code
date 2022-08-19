using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWriter : MonoBehaviour
{
    [HideInInspector]
    public bool isRunning = false;
    [HideInInspector]
    public bool isSpedUp = false;

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

    public void StartDisplayText(DialogueIteration _dialogueItrToUse, DialogueModule _caller, bool _autoIterate = false)
    {
        if (!isRunning)
        {
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
            TextBox.text = string.Empty;
            _caller.IncrementDialogueIteration();
            textIterator = 0;
        }
        else if (textIterator == _dialogueItrToUse.Dialogue.Count)
        {
            TextBox.text = string.Empty;
            textIterator = 0;
        }
        isRunning = false;
        isSpedUp = false;
    }
}
