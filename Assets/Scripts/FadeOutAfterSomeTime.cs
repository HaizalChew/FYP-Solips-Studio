using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAfterSomeTime : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float delay = 3f, refreshRate = 0.05f, fadeOutRate = 0.2f;

    public void FadeOut()
    {
        StartCoroutine(FadeOutAfterDelay());
    }

    private IEnumerator FadeOutAfterDelay()
    {
        _canvasGroup.alpha = 1;

        yield return new WaitForSeconds(delay);
        float counter = 1;

        while (_canvasGroup.alpha > 0)
        {
            counter -= fadeOutRate;
            _canvasGroup.alpha = counter;

            yield return new WaitForSeconds(refreshRate);
        }

        yield return new WaitForSeconds(delay);

        _canvasGroup.gameObject.SetActive(false);
    }
}
