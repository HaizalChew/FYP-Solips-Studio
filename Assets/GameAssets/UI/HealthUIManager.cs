using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform healthUIObj;
    [SerializeField] private RectTransform healthUIRect, healthUIDamageHoldRect;
    [SerializeField] private Health health;
    [SerializeField] private float whiteBarDuration = 1f;

    // Debugging
    [Header("Debug")]
    [SerializeField] private float healthWidth;
    [SerializeField] private float healthNormalised, healthNormalisedInterpolated, healthWidthWhiteCurrent;
    [SerializeField] private bool enableLookAtCamera = true;

    private bool isLerping = false;

    // Start is called before the first frame update
    void Start()
    {
        healthWidth = healthUIRect.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Update health in UI by checking health script
        UpdateHealth();

        // Forces the UI to always face the camera
        if (enableLookAtCamera)
        {
            LookAtCamera();
        }
    }

    private void UpdateHealth()
    {
        // calculate normalised value of enemy health
        healthNormalised = (float)health.health / (float)health.maxHealth;
        
        healthWidthWhiteCurrent = healthUIDamageHoldRect.sizeDelta.x / healthWidth;

        if (healthNormalisedInterpolated != healthNormalised && !isLerping)
        {
            StartCoroutine(LerpValues(healthWidthWhiteCurrent, healthNormalised, whiteBarDuration));
        }

        healthUIRect.sizeDelta = new Vector2(healthNormalised * healthWidth, healthUIRect.sizeDelta.y);
        healthUIDamageHoldRect.sizeDelta = new Vector2(healthNormalisedInterpolated * healthWidth, healthUIDamageHoldRect.sizeDelta.y);
    }

    private void LookAtCamera()
    {
        healthUIObj.transform.LookAt(healthUIObj.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private IEnumerator LerpValues(float startValue, float endValue, float lerpDuration)
    {
        float timeElapsed = 0;
        isLerping = true;

        while (timeElapsed < lerpDuration)
        {
            healthNormalisedInterpolated = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
        isLerping = false;
        //valueToLerp = endValue;
        //yield return valueToLerp;
    }
}
