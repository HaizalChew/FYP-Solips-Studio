using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float delay = 1;

    private void Start()
    {
        Invoke(nameof(DestroyThisShit), delay);
    }

    void DestroyThisShit()
    {
        Destroy(gameObject);
    }
}
