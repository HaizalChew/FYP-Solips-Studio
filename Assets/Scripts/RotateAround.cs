using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private float rotateSpeed = 45f;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
