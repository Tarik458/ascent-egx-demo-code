using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{

    [SerializeField]
    private Slider MainVolumeSlider;

    [SerializeField]
    private Slider MusicVolumeSlider;

    [SerializeField]
    private Slider SpeechVolumeSlider;

    private OptionsAndSettings optionsAndSettings;

    private void Awake()
    {
        optionsAndSettings = FindObjectOfType<OptionsAndSettings>();
        MainVolumeSlider.value = optionsAndSettings.GetMainVolume();
        MusicVolumeSlider.value = optionsAndSettings.GetMusicVolume();
        SpeechVolumeSlider.value = optionsAndSettings.GetSpeechVolume();

        MainVolumeSlider.onValueChanged.AddListener(optionsAndSettings.SetMainVolume);
        MusicVolumeSlider.onValueChanged.AddListener(optionsAndSettings.SetMusicVolume);
        SpeechVolumeSlider.onValueChanged.AddListener(optionsAndSettings.SetSpeechVolume);
    }

    private void OnDisable()
    {
        optionsAndSettings.SaveSettings();
    }
}
