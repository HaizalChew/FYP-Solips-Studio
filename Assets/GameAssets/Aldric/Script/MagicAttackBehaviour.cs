using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MagicAttackBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VisualEffect WarningVfx;
    [SerializeField] private VisualEffect MagicVfx;

    [Header("Variables")]
    [SerializeField] private float warningDuration = 0.5f;
    [SerializeField] private float warningSize = 5f, damageTime = 0.15f;
    [SerializeField] private int damage = 10;
    [SerializeField] private bool isCurrentlyDamaging = false, playerHasTakenDamage = false;

    private List<GameObject> hitList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        WarningVfx.SetFloat("Duration", warningDuration);
        WarningVfx.SetFloat("Size", warningSize);

        WarningVfx.Play();
        Invoke(nameof(PlayMagicEffect), warningDuration);
    }

    private void Update()
    {
        if (hitList.Count > 0 && !playerHasTakenDamage)
        {
            foreach (GameObject obj in hitList)
            {
                if (obj.GetComponent<Health>() != null)
                {
                    Health health = obj.GetComponent<Health>();

                    health.TakeDamage(damage);
                }
            }

            playerHasTakenDamage = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hitList.Contains(other.transform.root.gameObject) && isCurrentlyDamaging && other.gameObject.tag == "Player")
        {
            hitList.Add(other.transform.root.gameObject);
        }
    }

    private void PlayMagicEffect()
    {
        MagicVfx.Play();
        isCurrentlyDamaging = true;
        Invoke(nameof(StopDamaging), damageTime);
    }

    private void StopDamaging()
    {
        isCurrentlyDamaging = false;
    }

}
