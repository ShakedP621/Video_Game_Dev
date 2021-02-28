using System;
using UnityEngine;
using UnityEngine.AI;

namespace UnityEditor
{
    internal class NavMeshBuilder
    {
    }
}

[RequireComponent(typeof(NavMeshAgent))]

public class Enemy : MonoBehaviour, IEntity
{
    [SerializeField] float RadarRange = 10f;

    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float enemyHP = 100f;
    //[SerializeField] private DamageReceiver player;

    [SerializeField] private float enemyDamage = 5f;

    [SerializeField] private float attackRate = 0.5f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject deadPrefab;
    
    [HideInInspector]
    public Transform target;

    [HideInInspector]
    public EnemySpawner es;
    NavMeshAgent agent; 
    private float nextAttackTime = 0f;

    private float delay = 2.0f;

    private float timeTotarget;
    // Start is called before the first frame update
    void Start()
    {
        //target = PlayerManager.instance.player.transform;
        //target = LevelBuilder.getPlayer().transform;
       // target = player.transform;
       timeTotarget = Time.timeSinceLevelLoad + delay;
    }

    
    private void FixedUpdate()
    {
          
        if (agent == null)
        {
            
            agent = GetComponent<NavMeshAgent>();
            agent.stoppingDistance = attackDistance;
            agent.speed = movementSpeed;
            
        }

        if (timeTotarget > Time.timeSinceLevelLoad)
        {
            Debug.Log(gameObject.activeInHierarchy);
            Debug.Log(gameObject.activeSelf);
            agent.SetDestination(target.position);
            timeTotarget = Time.timeSinceLevelLoad + delay;
        }

        
    }
    
    
    // Update is called once per frame
    void Update()
    {

        timeTotarget -= Time.deltaTime;
        if(transform.position.y < -30.0f)
        {
            ApplyDamage(200);
        }

        float distance = Vector3.Distance(target.position, transform.position);
        /*
        if (distance > RadarRange )
        {
            //Physics.Raycast(Vector3.zero, Vector3.up, 0, 0);
        }
        */
        

        if (distance <= RadarRange)
        {
            
            if (distance <= agent.stoppingDistance)
            {
                if (Time.time > nextAttackTime)
                {
                    nextAttackTime = Time.time + attackRate;
                    RaycastHit hit;
                    if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackDistance))
                    {
                        if (hit.transform.CompareTag("Player"))
                        {
                            Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.red);
                            IEntity player = hit.transform.GetComponent<IEntity>();
                            player.ApplyDamage(enemyDamage);
                        }
                    }
                }

            }
        }
    }

    public void ApplyDamage(float points)
    {
        enemyHP -= points;
        if (enemyHP <= 0 )
        {
            GameObject dead = Instantiate(deadPrefab, transform.position, transform.rotation);
            //dead.GetComponent<Rigidbody>().velocity =
                //(-(playerTransform.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
            Destroy(dead, 10);
            es.EnemiesEliminated(this);
            Destroy(gameObject);
        }
    }
}
