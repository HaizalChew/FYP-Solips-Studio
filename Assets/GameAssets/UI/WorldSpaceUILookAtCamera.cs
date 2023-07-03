using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceUILookAtCamera : MonoBehaviour
{
    // Update is called once per frame
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
    }
    void Update()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
}
