using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlashShooter : MonoBehaviour
{

    public Camera cam;
    public GameObject projectile;
    //public GameObject trail;
    public Transform firePoint;
    public float fireRate = 4;

    private Vector3 destination;
    private float timeToFire;
    private GroundSlash groundSlashScript;  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1") && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        destination = ray.GetPoint(1000);
        InstantiateProjectile();
    }

    void InstantiateProjectile()
    {
        var projectileObj = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject;

        groundSlashScript = projectileObj.GetComponent<GroundSlash>();
        RotateToDestination(projectileObj, destination, true);
        projectileObj.GetComponent<Rigidbody>().velocity = transform.forward * groundSlashScript.speed;

        //var trailObj = Instantiate(trail, firePoint.position, Quaternion.identity) as GameObject;

        //groundSlashScript = trailObj.GetComponent<GroundSlash>();
        //RotateToDestination(trailObj, destination, true);
        //trailObj.GetComponent<Rigidbody>().velocity = transform.forward * groundSlashScript.speed;
    }

    void RotateToDestination(GameObject obj, Vector3 destination, bool onlyY)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation(direction);

        if (onlyY)
        {
            rotation.x = 0;
            rotation.z = 0;
        }

        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
}
