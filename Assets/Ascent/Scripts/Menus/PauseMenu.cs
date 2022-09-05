using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseMenuObj;

    [SerializeField]
    private GameObject OptionsMenuObj;


    /// <summary>
    /// Use Controls not _controls
    /// </summary>
    private MenuInputs _controls;
    /// <summary>
    /// Use Controls not _controls
    /// </summary>
    private MenuInputs Controls
    {
        get
        {
            if (_controls != null)
            {
                return _controls;
            }
            return _controls = new MenuInputs();
        }
    }

    private void Start()
    {
        Controls.PauseMenu.Pause_Unpause.performed += ctx => ShowHidePauseMenu();
    }


    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }


    private void ShowHidePauseMenu()
    {
        if (!PauseMenuObj.activeInHierarchy && OptionsMenuObj.activeInHierarchy)
        {
            OptionsMenuObj.SetActive(false);
            PauseMenuObj.SetActive(true);
        }
        else if (!PauseMenuObj.activeInHierarchy)
        {
            PauseMenuObj.SetActive(true);
        }
        else
        {
            PauseMenuObj.SetActive(false);
        }
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
