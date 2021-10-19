using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState { Search, RunAway, Hunt };
    public GameObject enemyDestination;
    Transform target;
    NavMeshAgent agent;

    public float lookRadius = 50f;
    public float enemyDistanceRun = 35f;

    public static int state;

    // Start is called before the first frame update
    void Start()
    {
        state = 0; // search
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        float distance;

        // doesnt need this 
        if (Input.GetKeyDown(KeyCode.Z))
        {
            state = (int)EnemyState.Search;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            state = (int)EnemyState.RunAway;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            state = (int)EnemyState.Hunt;
        }
        // doesnt need above code

        switch (state)
        {
            case (int) EnemyState.Search: // wander and search for weapons
                {
                    agent.SetDestination(enemyDestination.transform.position);
                    break;
                }

            case (int)EnemyState.RunAway: // run away from player
                {
                    distance = Vector3.Distance(transform.position, target.transform.position);
                    
                    if(distance < enemyDistanceRun)
                    {
                        Vector3 directionToPlayer = transform.position - target.transform.position;
                        Vector3 newPosistion = transform.position + directionToPlayer;
                        agent.SetDestination(newPosistion);
                    }

                    break;
                }

            case (int)EnemyState.Hunt: // hunt the player
                {
                    distance = Vector3.Distance(target.transform.position, transform.position);

                    if (distance <= lookRadius)
                    {
                        agent.SetDestination(target.position);

                        if (distance <= agent.stoppingDistance)
                        {
                            // attack target
                            FaceTarget();
                        }
                    }

                    break;
                }

            default:
                Debug.Log("EnemyController script - switch case is DEFAULT");
                break;
        }

    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
