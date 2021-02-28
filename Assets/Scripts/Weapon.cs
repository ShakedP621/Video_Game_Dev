using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private bool singleFire = false;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletsPerMag = 30; 
    [SerializeField] private Transform firePoint;

    [SerializeField] private float timeToReload = 1.5f;

    [SerializeField] private float weaponDamage = 15;



    private float nextFireTime = 0;

    private bool canFire = true;

    private int bulletsPerMagDefault = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        bulletsPerMagDefault = bulletsPerMag;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && singleFire)
            fire();
        if (Input.GetMouseButton(0) && !singleFire)
            fire();
        if (Input.GetKeyDown(KeyCode.R) && canFire)
            StartCoroutine(Reload());
    }


    void fire()
    {
        if (canFire)
        {
            if (Time.time > nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                
                if (bulletsPerMag > 0)
                {
                    Vector3 firePointPointerPosition = playerCamera.transform.position +
                                                       playerCamera.transform.forward * 100;
                    RaycastHit hit;
                    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100))
                        firePointPointerPosition = hit.point;
                    
                    firePoint.LookAt(firePointPointerPosition);

                    GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Bullet bullet = bulletObject.GetComponent<Bullet>();

                    bullet.SetDamage(weaponDamage);
                    bulletsPerMag--;
                 
                }
                else
                {
                    StartCoroutine(Reload());
                }
            }
        }
    }

    IEnumerator Reload()
    {
        canFire = false;
        yield return new WaitForSeconds(timeToReload);
        bulletsPerMag = bulletsPerMagDefault;
        canFire = true;
    }
    
}
