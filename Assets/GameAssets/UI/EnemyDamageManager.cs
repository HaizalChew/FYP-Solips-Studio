using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyDamageManager : MonoBehaviour
{
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Health health;
    [SerializeField] private float randomOffset = 1f;

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
        Vector3 randomiseVec3Offset = new Vector3(Random.Range(-randomOffset, randomOffset), Random.Range(-randomOffset, randomOffset), Random.Range(-randomOffset, randomOffset));
        GameObject damageTextInstance = Instantiate(damageTextPrefab);
        damageTextInstance.transform.position = GetComponentInChildren<Renderer>().bounds.center + randomiseVec3Offset;
        damageTextInstance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(damage.ToString());
    }
}
