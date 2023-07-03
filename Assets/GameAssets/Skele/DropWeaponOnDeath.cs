using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeaponOnDeath : MonoBehaviour
{
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
    }   
}
