using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region Singleton
    public static EnemyController instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    Transform player;
    NavMeshAgent agent;
    public GameObject destination;
    private AudioSource stepSound;
    

    public float lookRadius = 50f;
    public float enemyDistanceRun = 35f;
    private float distance, x, lx,  z, lz, stepLen = 0.1f;
    
    public static EnemyState state;


    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        stepSound = GetComponent<AudioSource>();
        stepSound.maxDistance = 50;
        stepSound.minDistance = 10;

        state = EnemyState.Wander;
        lx = transform.position.x;
        lz = transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(transform.position, player.transform.position);


        /*        // doesnt need this 
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    state = (int)EnemyState.Wander;
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    state = (int)EnemyState.RunAway;
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    state = (int)EnemyState.Hunt;
                }
                // doesnt need above code*/

        switch (state)
        {
            case EnemyState.Wander: // wander in terrain
                {

                    agent.SetDestination(destination.transform.position);
                    
                    break;
                }

            case EnemyState.RunAway: // run away from player
                {
                    /*

                    simple algorithm:
                            wander in map
                            if CanSee(player):
                                get away from player

                    */


                    
                    // old code
                    if(distance < enemyDistanceRun)
                    {
                        Vector3 directionToPlayer = transform.position - player.transform.position;
                        Vector3 newPosistion = transform.position + directionToPlayer;
                        agent.SetDestination(newPosistion);
                    }

                    break;
                }

            case EnemyState.Hunt: // hunt the player
                {
                    /*
                     
                    simple algorithm:
                            
                            if player not alive:
                                if PlayersHelper not alive:    
                                    gameOver
                                else:
                                    player = PlayersHelper

                            wander randomly in map        
                            if enemy CanSee(player):
                                if player CanShoot(player):
                                    FacePlayer
                                    ShootPlayer
                                else:
                                    get closer to player
                     
                     */




                    // old code
                    distance = Vector3.Distance(player.transform.position, transform.position);

                    if (distance <= lookRadius)
                    {
                        agent.SetDestination(player.position);

                        if (distance <= agent.stoppingDistance)
                        {
                            // attack target
                            FaceTarget();
                        }
                    }

                    break;
                }
            case EnemyState.Dead: // enemy is dead
                {
                    /* 
                     * 
                     * set animation to DEAD
                       if helper is alive:
                            helper = main enemy
                       else
                            gameOver
                     
                    */

                    break;
                }

            default:
                Debug.Log("EnemyController script - switch case is DEFAULT");
                break;
        }

        x = transform.position.x;
        z = transform.position.z;
        float dx = lx - x, dz = lz - z;

        if (dx < -stepLen || dx > stepLen || dz < -stepLen || dz > stepLen)
        {
            if (!stepSound.isPlaying)
            {
/*                if (distance < 100)
                {
                    
                    stepSound.Play();
                }*/
                    
            }
        }

    }

    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void UpdateEnemyState(EnemyState newState)
    {
        state = newState;
    }
}

public enum EnemyState 
{ 
    Wander, 
    RunAway, 
    Hunt, 
    Dead 
};
