using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainTitleOpacity;
    [SerializeField] private float fadeInRate = 0.2f, refreshRate = 0.05f, waitDelay = 2f;

    private void Start()
    {
        mainTitleOpacity.alpha = 0;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(waitDelay);

        float counter = 0;

        while (mainTitleOpacity.alpha < 1)
        {
            counter += fadeInRate;
            mainTitleOpacity.alpha = counter;

            yield return new WaitForSeconds(refreshRate);
        }
    }

    public void StartGame()
    {
        Debug.Log("Launching Game...");
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Qutting Application");
        Application.Quit();
    }
}
