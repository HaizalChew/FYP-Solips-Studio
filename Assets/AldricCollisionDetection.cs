using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AldricCollisionDetection : MonoBehaviour
{
    public AldricAI aldricAI;

    private void Start()
    {
        aldricAI = transform.root.GetComponent<AldricAI>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (aldricAI.isCurrentlyAttacking && other.gameObject.tag == "Player")
        {
            Debug.Log("Hit " + other.transform.root.gameObject.name);

            if (!aldricAI.hitColliders.Contains(other.transform.root.gameObject))
            {
                aldricAI.hitColliders.Add(other.transform.root.gameObject);
            }
        }
    }
}
