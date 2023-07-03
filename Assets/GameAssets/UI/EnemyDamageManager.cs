using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDamageManager : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Health health;

    private void Start()
    {
        if (health == null)
        {
            health = GetComponent<Health>();
        }
    }

    private void Update()
    {
        if (health.isHit)
        {
            ShowDamageUI(health.storeDamage);
            health.isHit = false;
        }
    }

    public void ShowDamageUI(int damage)
    {       
        GameObject damageTextInstance = Instantiate(damageTextPrefab);
        damageTextInstance.transform.position = GetComponentInChildren<Renderer>().bounds.center;
        damageTextInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(damage.ToString());
    }
}
