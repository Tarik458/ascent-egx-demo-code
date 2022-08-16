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
    private List<DialogueIteration> DialogueIterations;

    [SerializeField]
    private TextMeshProUGUI TextBox;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private IEnumerator DisplayText(int _index)
    {
        string strToDisplay;
        AudioSource.Stop();

        for (int i = 0; i < DialogueIterations[0].Dialogue[_index].Length; i++)
        {
            strToDisplay = DialogueIterations[0].Dialogue[_index].Substring(0, i);
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
