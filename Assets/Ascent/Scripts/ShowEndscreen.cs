using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowEndscreen : MonoBehaviour
{
    [SerializeField]
    private GameObject EndScreenCanvas;


    public void ShowCanvas()
    {
        EndScreenCanvas.SetActive(true);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
