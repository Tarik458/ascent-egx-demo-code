using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowEndscreen : MonoBehaviour
{
    [SerializeField]
    private GameObject EndScreenCanvas;

    [SerializeField]
    private float FadeInTime = 2.5f;

    [SerializeField]
    AudioSource audioSource;

    public void ShowCanvas()
    {

        StartCoroutine(FadeInCanvas());
        audioSource.Play();
    }

    private IEnumerator FadeInCanvas()
    {
        EndScreenCanvas.SetActive(true);
        CanvasGroup canvasGroup = EndScreenCanvas.GetComponent<CanvasGroup>();
        float startVal = 0f;
        float endval = 1f;
        float timeElapsed = 0f;
        canvasGroup.alpha = startVal;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha = Mathf.Lerp(startVal, endval, timeElapsed / FadeInTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endval;
    }



    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
