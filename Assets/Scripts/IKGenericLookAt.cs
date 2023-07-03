using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using UnityEngine.Experimental.Animations;

public class IKGenericLookAt : MonoBehaviour
{
    [SerializeField] private Transform joint;
    [SerializeField] private Transform target;
    [SerializeField] private Transform fixedHeadPoint;
    [SerializeField] private float fieldOfViewAngle = 60.0f;
    [SerializeField] private float rotateSpeed = 5.0f;
    [SerializeField] private bool isLookAtActive = false;

    private void LateUpdate()
    {
        isLookAtActive = IsInFieldOfView();

        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsTag("UnmoveNohead"))
        {
            if (isLookAtActive)
            {
                Debug.Log("Looking!!!!");
                LookAtTarget(target.position);
            }
            else
            {
                LookAtTarget(fixedHeadPoint.position);
            }
        }
    }

    private void LookAtTarget(Vector3 targetPos)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = targetPos - joint.position;

        // The step size is equal to speed times frame time.
        float singleStep = rotateSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(joint.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(joint.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        joint.rotation = Quaternion.LookRotation(newDirection);
    }

    private bool IsInFieldOfView()
    {
        // Determine which direction to rotate towards
        Vector3 dir = target.position - joint.position;

        if (Mathf.Abs(Vector3.Angle(transform.forward, dir)) < fieldOfViewAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
