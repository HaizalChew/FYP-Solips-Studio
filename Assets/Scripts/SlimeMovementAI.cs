using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovementAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed, jumpCooldown, maxAgroDistance, rotationSpeed;
    [SerializeField] private GameObject player, rootRbObj, bottomRbObj;
    [SerializeField] private Rigidbody rootRb, bottomRb;

    private bool canJump = true;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        LookAt();

        if (canJump && Vector3.Distance(transform.position, player.transform.position) < maxAgroDistance)
        {
            canJump = false;
            Move();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    private void Move()
    {
        // Rotate towards player
        Vector3 RotateDir = player.transform.position - transform.position;

        // Jump towards player with rigidbody
        rootRb = rootRbObj.GetComponent<Rigidbody>();
        bottomRb = bottomRbObj.GetComponent<Rigidbody>();

        Vector3 moveDir = moveSpeed * (transform.forward + Vector3.up);
        rootRb.AddForce(moveDir, ForceMode.Impulse);
        //bottomRb.AddForce(Vector3.up * moveSpeed, ForceMode.Force);
    }
    private void ResetJump()
    {
        canJump = true;
    }

    private void LookAt()
    {
        // Get direction
        Vector3 targetDir = player.transform.position - transform.position;

        // Rotate Slime to look at player
        Vector3 newDir = Vector3.RotateTowards(transform.position, targetDir, rotationSpeed * Time.deltaTime, 0.0f);

        Debug.DrawRay(transform.position, newDir, Color.red);
        // Calculate Rotation
        transform.rotation = Quaternion.LookRotation(newDir);
    }
}
