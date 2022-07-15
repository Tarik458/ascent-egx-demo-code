using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsAndSettings : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The main audio mixer, used for applying volume adjustments.")]
    private AudioMixer audioMixer;

    // Actual available resolutions.
    private Resolution[] resolutions;
    private int currentResolutionIndex;
    /// <summary>
    /// Resolution string for display purposes.
    /// </summary>
    private List<string> resolutionOptions = new();

    private bool fullScreenEnabled;

    private float mainVolume;
    private float musicVolume;
    private float speechVolume;

    // Changed settings.
    private bool resChanged = false;
    private bool fullScrnChanged = false;

    private bool mainVolumeChanged = false;
    private bool musicVolumeChanged = false;
    private bool speechVolumeChanged = false;

    private void Awake()
    {
        // cehck for save file -> if yes then load it
        // if not found save file

        // Gets a list of available resolution options.
        resolutions = Screen.resolutions;
        // Cycles through list to find current resolution.
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                SetResolution(i);
            }
        }

        // save resolutions and current resolution if not loaded

    }

    /// <summary>
    /// Checks for changed settings and if their value was changed saves them.
    /// </summary>
    public void SaveSettings()
    {

        if (resChanged)
        {
            PlayerPrefs.SetInt("ResolutionPref", currentResolutionIndex);
            resChanged = false;
        }
        if (fullScrnChanged)
        {
            PlayerPrefs.SetInt("FullScreenPref", BoolToInt(fullScreenEnabled));
            fullScrnChanged = false;
        }
        if (mainVolumeChanged)
        {
            PlayerPrefs.SetFloat("MainVolumePref", mainVolume);
            mainVolumeChanged = false;
        }
        if (musicVolumeChanged)
        {
            PlayerPrefs.SetFloat("MusicVolumePref", musicVolume);
            musicVolumeChanged = false;
        }
        if (speechVolumeChanged)
        {
            PlayerPrefs.SetFloat("SpeechVolumePref", speechVolume);
            speechVolumeChanged = false;
        }

        PlayerPrefs.Save();
    }


    public void SetResolution(int _resIndex)
    {
        Screen.SetResolution(resolutions[_resIndex].width, resolutions[_resIndex].height, Screen.fullScreenMode);
        currentResolutionIndex = _resIndex;
        resChanged = true;
    }

    public void SetFullScreen(bool _enabled)
    {
        fullScreenEnabled = _enabled;
        Screen.fullScreen = fullScreenEnabled;
        fullScrnChanged = true;
    }

    public void SetMainVolume(float _volume)
    {
        mainVolume = _volume;
        audioMixer.SetFloat("MainVolume", mainVolume);
        mainVolumeChanged = true;
    }

    public void SetMusicVolume(float _volume)
    {
        musicVolume = _volume;
        audioMixer.SetFloat("MusicVolume", musicVolume);
        musicVolumeChanged = true;
    }

    public void SetSpeechVolume(float _volume)
    {
        speechVolume = _volume;
        audioMixer.SetFloat("SpeechVolume", speechVolume);
        speechVolumeChanged = true;
    }

    /// <summary>
    /// Returns '1' if input bool is 'true' and '0' if input bool is 'false'.
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    private int BoolToInt(bool _input)
    {
        if (_input)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// Returns 'true' if input int is '1' and 'false' if input int is '0'.
    /// </summary>
    /// <param name="_input"></param>
    /// <returns></returns>
    private bool IntToBool(int _input)
    {
        if (_input == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
