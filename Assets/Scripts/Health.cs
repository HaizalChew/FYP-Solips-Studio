using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Place this script in root of GameObject
public class Health : MonoBehaviour
{
    public Animator animator;
    public int maxHealth;
    public int health;
    public int storeDamage;

    public bool isDead = false, isHit = false;

    [SerializeField] private UnityEvent OnDeath, OnTakeDamage;

    private void Start()
    {
        health = maxHealth;
        if (this.GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>();
        }
        
    }

    private void Update()
    {
        if (health <= 0 && !isDead)
        {
            TriggerDeath();
            OnDeath.Invoke();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        storeDamage = damage;
        isHit = true;

        if (animator != null)
        {
            OnTakeDamage.Invoke();
        }
    }

    public void RecoverHealth(int heal)
    {
        health -= heal;
    }

    public void TriggerDeath()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetBool("IsDead", true);
        }
        
    }

}
