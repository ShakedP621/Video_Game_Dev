﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 345.0f;
    [SerializeField] private float hitForce = 50.0f;
    [SerializeField] private float destroyAfter = 3.5f;

    float currentTime = 0; 
    Vector3 newPos;
    Vector3 oldPos;
    private bool hasHit = false;
    private float damagePoints;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        newPos = transform.position;
        oldPos = newPos;

        while (currentTime < destroyAfter && !hasHit)
        {
            Vector3 velocity = transform.forward * bulletSpeed;
            newPos += velocity * Time.deltaTime;
            Vector3 direction = newPos - oldPos;
            float distance = direction.magnitude;
            RaycastHit hit;

            if (Physics.Raycast(oldPos , direction, out hit, distance))
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(direction * hitForce);
                    IEntity npc = hit.transform.GetComponent<IEntity>();
                    if (npc != null)
                    {
                        npc.ApplyDamage(damagePoints);
                    }
                }

                newPos = hit.point;
            }

            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
            transform.position = newPos;
            oldPos = newPos;
        }

        if (!hasHit)
        {
            StartCoroutine(DestroyBullet());
        }
    }

    IEnumerator DestroyBullet()
    {
        hasHit = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void SetDamage(float points)
    {
        damagePoints = points;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}