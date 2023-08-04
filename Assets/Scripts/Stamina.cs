using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public int maxStamina;
    public int stamina;
    public float staminaRecoveryRate = 0.2f;
    public float staminaDelayBeforeRecovery = 2f;
    public bool isPassiveStaminaRecovering;

    private void Start()
    {
        stamina = maxStamina;
    }

    private void Update()
    {
        if (!isPassiveStaminaRecovering && stamina < maxStamina)
        {
            isPassiveStaminaRecovering = true;
            StartCoroutine(RecoverStamina(staminaDelayBeforeRecovery, staminaRecoveryRate));
        }
        else if (stamina >=              maxStamina)
        {
            isPassiveStaminaRecovering = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator RecoverStamina(float delay, float rate)
    {
        yield return new WaitForSeconds(delay);

        while (stamina < maxStamina)
        {
            stamina++;
            yield return new WaitForSeconds(rate);
        }
    }

    public void ConsumeStamina(int cost)
    {
        stamina -= cost;
        StopAllCoroutines();
        isPassiveStaminaRecovering = false;
    }

}
