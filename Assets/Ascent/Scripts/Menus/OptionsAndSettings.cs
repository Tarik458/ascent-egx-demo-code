using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsAndSettings : MonoBehaviour
{

    // Actual available resolutions.
    private Resolution[] resolutions;
    private int currentResolutionIndex;
    /// <summary>
    /// Resolution string for display purposes.
    /// </summary>
    private List<string> resolutionOptions = new();

    private bool resChanged = false;


    private void Awake()
    {
        // cehck for save file -> if yes then load it
        // if not found save file

        resolutions = Screen.resolutions;

        // Gets a list of available resolution options.
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                SetResolution(i);
            }
        }
        // save resolutions and current resolution

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


        PlayerPrefs.Save();
    }


    public void SetResolution(int _resIndex)
    {
        Screen.SetResolution(resolutions[_resIndex].width, resolutions[_resIndex].height, Screen.fullScreenMode);
        currentResolutionIndex = _resIndex;
        resChanged = true;
    }





}
