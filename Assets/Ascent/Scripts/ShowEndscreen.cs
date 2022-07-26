using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowEndscreen : MonoBehaviour
{
    [SerializeField]
    private GameObject EndScreenCanvas;

    [SerializeField]
    AudioSource audioSource;

    public void ShowCanvas()
    {
        EndScreenCanvas.SetActive(true);
        audioSource.Play();
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
