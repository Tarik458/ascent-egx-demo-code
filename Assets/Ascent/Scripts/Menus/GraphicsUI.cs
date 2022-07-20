using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsUI : MonoBehaviour
{

    [SerializeField]
    private Dropdown ResolutionDropdown;

    [SerializeField]
    private Dropdown QualityDropdown;

    [SerializeField]
    private Toggle FullScreenToggle;

    private OptionsAndSettings optionsAndSettings;

    private void Awake()
    {
        optionsAndSettings = FindObjectOfType<OptionsAndSettings>();

        ResolutionDropdown.ClearOptions();
        ResolutionDropdown.AddOptions(optionsAndSettings.GetResolutionList());
        ResolutionDropdown.value = optionsAndSettings.GetResolutionIndex();

        QualityDropdown.value = optionsAndSettings.GetQualityLevel();

        FullScreenToggle.isOn = optionsAndSettings.GetFullScreen();
    }
}
