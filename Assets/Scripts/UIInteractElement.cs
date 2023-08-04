using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class UIInteractElement : MonoBehaviour
{
    [SerializeField] private CanvasGroup uiElement;
    [SerializeField] private InputActionReference Select;
    [SerializeField] private UnityEvent OnPressedSelect;
    [SerializeField] private float fadeInRate = 0.02f, refreshrate = 0.05f;
    [SerializeField] private bool isEventTriggered, isRunning;

    private void OnTriggerEnter(Collider other)
    {
        if (!isRunning && other.CompareTag("Player") && !isEventTriggered)
        {
            StartCoroutine(FadeIn());
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(Select.action.IsPressed() && !isEventTriggered && other.CompareTag("Player"))
        {
            isEventTriggered = true;
            OnPressedSelect.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isRunning && other.CompareTag("Player") && !isEventTriggered)
        {
            
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        uiElement.gameObject.SetActive(true);

        isRunning = true;
        float counter = 0;

        while (uiElement.alpha < 1)
        {
            counter += fadeInRate;
            uiElement.alpha = counter;

            yield return new WaitForSeconds(refreshrate);
        }
        
        isRunning = false;
    }

    IEnumerator FadeOut()
    {
        isRunning = true;
        float counter = 1;

        while (uiElement.alpha > 0)
        {
            counter -= fadeInRate;
            uiElement.alpha = counter;

            yield return new WaitForSeconds(refreshrate);
        }

        uiElement.gameObject.SetActive(false);
        isRunning = false;
    }

}
