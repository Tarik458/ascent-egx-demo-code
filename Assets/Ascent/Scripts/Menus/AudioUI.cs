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

    private void Awake()
    {
        
    }
}
