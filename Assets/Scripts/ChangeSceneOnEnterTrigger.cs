using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;

public class ChangeSceneOnEnterTrigger : MonoBehaviour
{
    [SerializeField] private VisualEffect smokeEffect;
    [SerializeField] private float delayBeforeChange = 4f;
    [SerializeField] private bool changingScenes;

    private void OnTriggerEnter(Collider other)
    {
        if (!changingScenes && other.CompareTag("Player"))
        {
            smokeEffect.Play();
            StartCoroutine(ChangeSceneOnDelay(delayBeforeChange));
        }
        
    }

    IEnumerator ChangeSceneOnDelay(float delay)
    {
        changingScenes = true;

        yield return new WaitForSeconds(delay);

        Debug.Log("Changing Scenes!");
        SceneManager.LoadScene("EverblossomScene");

    }
}
