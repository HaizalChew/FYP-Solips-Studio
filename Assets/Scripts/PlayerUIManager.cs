using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject lockOnTarget;
    [SerializeField] private Camera mainCam;
    [SerializeField] private RectTransform lockOnUI;
    [SerializeField] private ThirdPersonCam thirdPersonCam;
    [SerializeField] private PlayerAttack playerAttack;

    private void Update()
    {
        lockOnTarget = playerAttack.nearestTarget;

        if (lockOnTarget != null && thirdPersonCam.currentCameraStyle == ThirdPersonCam.CameraStyle.Combat)
        {
            lockOnUI.gameObject.SetActive(true);
            LockOnTarget(lockOnTarget);
        }
        else
        {
            lockOnUI.gameObject.SetActive(false);
        }
        
    }

    private void LockOnTarget(GameObject target)
    {
        // Always set character skeleton as the first child 
        Vector3 center = target.transform.GetChild(0).position;
        Vector3 screenPos = mainCam.WorldToScreenPoint(center);

        lockOnUI.position = screenPos;


    }
}
