using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject iceShardObj;
    [SerializeField] private Transform shootingPoint;

    [Header("Variables")]
    [SerializeField] private InputActionReference basicAttack;
    [SerializeField] private InputActionReference equipWeapon, aimDownSight;
    [SerializeField] private float basicAtkCooldown;
    [SerializeField] private int basicAtkDamage;
    [SerializeField] private ThirdPersonCam thirdPersonCam;

    [Header("LockOn")]
    [SerializeField] public GameObject nearestTarget;
    [SerializeField] private float noticeZone = 10f;
    [SerializeField] private LayerMask targetLayers;
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 60;

    [Header("Debug")]
    [SerializeField] private bool canAttack = true, isInAttackState = false; 

    public bool isBasicAttacking = false;
    public List<GameObject> hitEnemyColliders = new List<GameObject>();

    private void OnEnable()
    {
        basicAttack.action.performed += ctx => BasicAttack();
        equipWeapon.action.performed += ctx => ToggleEquip();
    }

    private void OnDisable()
    {
        basicAttack.action.performed -= ctx => BasicAttack();
        equipWeapon.action.performed -= ctx => ToggleEquip();
    }

    private void Update()
    {
        if (!isBasicAttacking && hitEnemyColliders.Count != 0)
        {
            foreach (GameObject obj in hitEnemyColliders)
            {
                if (obj.GetComponent<Health>() != null)
                {
                    Health health = obj.GetComponent<Health>();
                    EnemyCombatAI enemyCombatScript = obj.GetComponent<EnemyCombatAI>();

                    if (enemyCombatScript.nullDamageFromShield == true)
                    {
                        health.TakeDamage(0);
                    }
                    else
                    {
                        health.TakeDamage(basicAtkDamage);
                    }
                    
                }
            }

            hitEnemyColliders.Clear();
        }

        if (aimDownSight.action.WasPressedThisFrame())
        {
            nearestTarget = ScanNearBy();
        }    
    }

    private void BasicAttack()
    {
        if (canAttack && isInAttackState)
        {
            canAttack = false;

            if (thirdPersonCam.currentCameraStyle == ThirdPersonCam.CameraStyle.Combat)
            {
                animator.SetTrigger("DoMagicAttack");
            }
            else
            {
                animator.SetTrigger("DoAttack");
            }
            
            Invoke(nameof(ResetAttack), basicAtkCooldown);
        }
        
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    // Toggle equip weapon
    private void ToggleEquip()
    {
        isInAttackState = !isInAttackState;
        animator.SetBool("IsAttackState", isInAttackState);

    }

    // Used with AnimationEvents; Do not change the names unless event names are changed
    public void BasicAttackStart()
    {
        isBasicAttacking = true;
    }

    public void BasicAttackEnd()
    {
        isBasicAttacking = false;
    }

    public void MagicAttackStart()
    {
        GameObject iceProjectile = Instantiate(iceShardObj, shootingPoint.position, shootingPoint.rotation);
        iceProjectile.GetComponent<ProjectileBehaviour>().target = nearestTarget;
    }

    public GameObject ScanNearBy()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        float closestAngle = maxNoticeAngle;
        GameObject closestTarget = null;

        if (nearbyTargets.Length <= 0) return null;

        for (int i = 0; i < nearbyTargets.Length; i++)
        {
            Vector3 dir = nearbyTargets[i].transform.position - Camera.main.transform.position;
            dir.y = 0;
            float _angle = Vector3.Angle(Camera.main.transform.forward, dir);

            if (_angle < closestAngle)
            {
                closestTarget = nearbyTargets[i].transform.root.gameObject;
                closestAngle = _angle;
            }
        }

        if (!closestTarget) return null;

        return closestTarget;
    }
}
