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

    private int qualityLevel;

    private bool isFullScreen;

    private float mainVolume;
    private float musicVolume;
    private float speechVolume;


    // Changed settings.
    private bool resChanged = false;
    private bool qualityChanged = false;
    private bool fullScrnChanged = false;

    private bool mainVolumeChanged = false;
    private bool musicVolumeChanged = false;
    private bool speechVolumeChanged = false;


    private static GameObject instance;

    private void Awake()
    {
        qualityLevel = QualitySettings.GetQualityLevel();

        // Keep only this object for the settings, destroy object if one already exists.
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this.gameObject;
        }

        // Gets a list of available resolution options.
        resolutions = Screen.resolutions;
        
        // Cycles through list to find current resolution.
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                if (PlayerPrefs.HasKey("ResolutionPref") == false)
                {
                    SetResolutionIndex(i);
                }
                else
                {
                    SetResolutionIndex(PlayerPrefs.GetInt("ResolutionPref"));
                }
            }
        }
    }

    private void Start()
    {
        LoadSettings();
    }

    /// <summary>
    /// Checks for changed settings and if their value was changed saves them.
    /// </summary>
    public void SaveSettings()
    {

        if (resChanged || PlayerPrefs.HasKey("ResolutionPref") == false)
        {
            PlayerPrefs.SetInt("ResolutionPref", currentResolutionIndex);
            resChanged = false;
        }
        if (qualityChanged || PlayerPrefs.HasKey("QualityPref") == false)
        {
            PlayerPrefs.SetInt("QualityPref", qualityLevel);
            qualityChanged = false;
        }
        if (fullScrnChanged || PlayerPrefs.HasKey("FullScreenPref") == false)
        {
            PlayerPrefs.SetInt("FullScreenPref", BoolToInt(isFullScreen));
            fullScrnChanged = false;
        }
        if (mainVolumeChanged || PlayerPrefs.HasKey("MainVolumePref") == false)
        {
            PlayerPrefs.SetFloat("MainVolumePref", mainVolume);
            mainVolumeChanged = false;
        }
        if (musicVolumeChanged || PlayerPrefs.HasKey("MusicVolumePref") == false)
        {
            PlayerPrefs.SetFloat("MusicVolumePref", musicVolume);
            musicVolumeChanged = false;
        }
        if (speechVolumeChanged || PlayerPrefs.HasKey("SpeechVolumePref") == false)
        {
            PlayerPrefs.SetFloat("SpeechVolumePref", speechVolume);
            speechVolumeChanged = false;
        }

        PlayerPrefs.Save();
    }

    public void  LoadSettings()
    {
        if (PlayerPrefs.HasKey("ResolutionPref"))
        {
            SetResolutionIndex(PlayerPrefs.GetInt("ResolutionPref")); 
        }

        if (PlayerPrefs.HasKey("QualityPref"))
        {
            SetQualityLevel(PlayerPrefs.GetInt("QualityPref"));
        }

        if (PlayerPrefs.HasKey("FullScreenPref"))
        {
            SetFullScreen(IntToBool(PlayerPrefs.GetInt("FullScreenPref")));
        }

        if (PlayerPrefs.HasKey("MainVolumePref"))
        {
            SetMainVolume(PlayerPrefs.GetFloat("MainVolumePref"));
        }

        if (PlayerPrefs.HasKey("MusicVolumePref"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolumePref"));
        }

        if (PlayerPrefs.HasKey("SpeechVolumePref"))
        {
            SetSpeechVolume(PlayerPrefs.GetFloat("SpeechVolumePref"));
        }
    }

    public List<string> GetResolutionList()
    {
        return resolutionOptions;
    }

    public void SetResolutionIndex(int _resIndex)
    {
        Screen.SetResolution(resolutions[_resIndex].width, resolutions[_resIndex].height, Screen.fullScreenMode);
        currentResolutionIndex = _resIndex;
        resChanged = true;
    }
    public int GetResolutionIndex()
    {
        return currentResolutionIndex;
    }

    public void SetQualityLevel(int _qualIndex)
    {
        qualityLevel = _qualIndex;
        QualitySettings.SetQualityLevel(qualityLevel, true);
        qualityChanged = true;
    }
    public int GetQualityLevel()
    {
        return qualityLevel;
    }

    public void SetFullScreen(bool _enabled)
    {
        isFullScreen = _enabled;
        Screen.fullScreen = isFullScreen;
        fullScrnChanged = true;
    }
    public bool GetFullScreen()
    {
        return isFullScreen;
    }


    public void SetMainVolume(float _volume)
    {
        mainVolume = _volume;
        audioMixer.SetFloat("Master", mainVolume);
        mainVolumeChanged = true;
    }
    public float GetMainVolume()
    {
        return mainVolume;
    }

    public void SetMusicVolume(float _volume)
    {
        musicVolume = _volume;
        audioMixer.SetFloat("Music", musicVolume);
        musicVolumeChanged = true;
    }
    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetSpeechVolume(float _volume)
    {
        speechVolume = _volume;
        audioMixer.SetFloat("Speech", speechVolume);
        speechVolumeChanged = true;
    }
    public float GetSpeechVolume()
    {
        return speechVolume;
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
