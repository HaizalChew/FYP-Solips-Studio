using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private InputActionReference select;

    private void OnEnable()
    {
        select.action.performed += ctx => RestartLevel();
    }

    private void OnDisable()
    {
        select.action.performed -= ctx => RestartLevel();
    }

    private void RestartLevel()
    {
        if (playerHealth.isDead)
        {
            Debug.Log("Scene is Restarting!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
