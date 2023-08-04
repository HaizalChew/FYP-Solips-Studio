using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineFreeLook thirdPersonCm;
    [SerializeField] private CinemachineFreeLook combatCm;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObj;

    [SerializeField] private InputActionReference movement, aimDownSight;

    [SerializeField] private float rotationSpeed;

    [SerializeField] public CameraStyle currentCameraStyle;
    [SerializeField] private Transform combatLookAt;
    [SerializeField] private GameObject cmBasicObj;
    [SerializeField] private GameObject cmCombatObj;
    [SerializeField] private GameObject targetObj;

    public enum CameraStyle
    {
        Basic,
        Combat
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Reset Camera press to frame
        if (aimDownSight.action.WasPressedThisFrame())
        {
            combatCm.m_YAxis.Value = thirdPersonCm.m_YAxis.Value;
            combatCm.m_XAxis.Value = thirdPersonCm.m_XAxis.Value;
        }

        if (aimDownSight.action.WasReleasedThisFrame())
        {
            thirdPersonCm.m_YAxis.Value = combatCm.m_YAxis.Value;
            thirdPersonCm.m_XAxis.Value = combatCm.m_XAxis.Value;
        }

        // check if player is trying to aim
        if (aimDownSight.action.IsPressed())
        {
            currentCameraStyle = CameraStyle.Combat;
        }
        else
        {
            currentCameraStyle = CameraStyle.Basic;
        }
        

        // rotate orientation
        Vector3 viewDir = playerObj.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        if (currentCameraStyle == CameraStyle.Basic)
        {
            // enable camera settings
            cmBasicObj.SetActive(true);
            cmCombatObj.SetActive(false);

            // rotate player object
            Vector2 movementInput = movement.action.ReadValue<Vector2>();
            Vector3 inputDir = orientation.forward * movementInput.y + orientation.right * movementInput.x;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                playerObj.eulerAngles = new Vector3(0f, playerObj.eulerAngles.y, playerObj.eulerAngles.z);
            }
        }

        else if (currentCameraStyle == CameraStyle.Combat)
        {
            // enable camera settings
            cmBasicObj.SetActive(false);
            cmCombatObj.SetActive(true);

            // rotate orientation
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }

    }

}
