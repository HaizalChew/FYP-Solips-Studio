using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform staminaUIObj;
    [SerializeField] private RectTransform staminaUIRect, staminaUIDamageHoldRect;
    [SerializeField] private Stamina stamina;
    [SerializeField] private float whiteBarDuration = 1f;

    // Debugging
    [Header("Debug")]
    [SerializeField] private float staminaWidth;
    [SerializeField] private float staminaNormalised, staminaNormalisedInterpolated, staminaWidthWhiteCurrent;
    [SerializeField] private bool enableLookAtCamera = true;

    private bool isLerping = false;

    // Start is called before the first frame update
    void Start()
    {
        staminaWidth = staminaUIRect.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Update health in UI by checking health script
        UpdateStamina();

        // Forces the UI to always face the camera
        if (enableLookAtCamera)
        {
            LookAtCamera();
        }
    }

    private void UpdateStamina()
    {
        // calculate normalised value of enemy health
        staminaNormalised = (float)stamina.stamina / (float)stamina.maxStamina;
        
        staminaWidthWhiteCurrent = staminaUIDamageHoldRect.sizeDelta.x / staminaWidth;

        if (staminaNormalisedInterpolated != staminaNormalised && !isLerping)
        {
            StartCoroutine(LerpValues(staminaWidthWhiteCurrent, staminaNormalised, whiteBarDuration));
        }

        staminaUIRect.sizeDelta = new Vector2(staminaNormalised * staminaWidth, staminaUIRect.sizeDelta.y);
        staminaUIDamageHoldRect.sizeDelta = new Vector2(staminaNormalisedInterpolated * staminaWidth, staminaUIDamageHoldRect.sizeDelta.y);
    }

    private void LookAtCamera()
    {
        staminaUIObj.transform.LookAt(staminaUIObj.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private IEnumerator LerpValues(float startValue, float endValue, float lerpDuration)
    {
        float timeElapsed = 0;
        isLerping = true;

        while (timeElapsed < lerpDuration)
        {
            staminaNormalisedInterpolated = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
        isLerping = false;
        //valueToLerp = endValue;
        //yield return valueToLerp;
    }
}
