using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInAfterSomeTime : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float delay = 3f, refreshRate = 0.05f, fadeOutRate = 0.2f;

    public void FadeIn()
    {
        StartCoroutine(FadeInAfterDelay());
    }

    private IEnumerator FadeInAfterDelay()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay);
        float counter = 0;

        while (_canvasGroup.alpha < 1)
        {
            counter += fadeOutRate;
            _canvasGroup.alpha = counter;

            yield return new WaitForSeconds(refreshRate);
        }

        yield return new WaitForSeconds(delay);


    }
}
