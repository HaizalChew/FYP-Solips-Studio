using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeaponOnDeath : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 10f;
    public void DropWeapon()
    {
        transform.parent = null;
        int layerNonInteractable = LayerMask.NameToLayer("NonInteractables");
        gameObject.layer = layerNonInteractable;

        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
        }

        if (gameObject.GetComponent<Collider>() == null)
        {
            MeshCollider col = gameObject.AddComponent<MeshCollider>();
            col.convex = true;
        }
        else
        {
            gameObject.GetComponent<Collider>().isTrigger = false;
        }

        Invoke(nameof(DestroyOverTime), destroyDelay);
    }
    
    void DestroyOverTime()
    {
        Destroy(gameObject);
    }
}
