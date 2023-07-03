using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] public GameObject target;

    [SerializeField] private float rotateSpeed = 95f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int damage = 10;


    private void Start()
    {
        if (rb != null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;

        if (target != null)
        {
            RotateRocket();
        }
        
    }

    private void RotateRocket()
    {
        var heading = target.transform.GetChild(0).position - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;
        //Instantiate(explosionPrefab, position, rotation);
        Destroy(gameObject);

        if (contact.otherCollider.tag == "Enemy")
        {
            contact.otherCollider.transform.root.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
