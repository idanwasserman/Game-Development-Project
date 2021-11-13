using System;
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

        //For good measure, set the previous locations
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }
    }
    #endregion

    private float time = 0f;
    private bool isDead = false, stop = false;

    private Transform player;
    private NavMeshAgent agent;
    public GameObject destination;
    public GameObject p;
    private AudioSource stepSound;
    

    public float lookRadius = 100f, shootRadius = 50f;
    public int damage = 10;
    public int hitRatio = 10;
    public float delayTime = 2.5f;
    public float enemyDistanceRun = 35f;
    private float distance, x, lx,  z, lz, stepLen = 0.1f;
    
    public static EnemyState state;

    // animations variables
    private Animator animator;
    private float noMovementThreshold = 0.025f;
    private const int noMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    private bool isMoving;
    // shooting
    private bool hasGun = false;
    private AudioSource shootingSound;


    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        shootingSound = GetComponent<AudioSource>();
        //stepSound.maxDistance = 50;
       // stepSound.minDistance = 10;

        state = EnemyState.Wander;
        lx = transform.position.x;
        lz = transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (isDead || stop)
        {
            return;
        }
        
        SetAnimation();
       
        StateBehave();
        
        MakeWalkingSound();
    }

    private void MakeWalkingSound()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        x = transform.position.x;
        z = transform.position.z;
        float dx = lx - x, dz = lz - z;

        if (dx < -stepLen || dx > stepLen || dz < -stepLen || dz > stepLen)
        {
            //  if (!stepSound.isPlaying)
            {
                /*                if (distance < 100)
                                {

                                    stepSound.Play();
                                }*/

            }
        }
    }

    private void StateBehave()
    {
        switch (state)
        {
            case EnemyState.Wander:
                WanderInTerrain();
                break;

            case EnemyState.RunAway:
                WanderInTerrain();
                break;

            case EnemyState.Hunt: // hunt the player
                HuntPlayer();
                break;

            case EnemyState.Dead: // enemy is dead
                Die();
                break;

            default:
                Debug.Log("EnemyController::StateBehave()-switch case is DEFAULT");
                break;
        }
    }

    private void Die()
    {
        isDead = true;
        agent.enabled = false;
        animator.SetInteger("States", (int)AnimationStates.Dead);
    }

    public void UpdateEnemyState(EnemyState newState)
    {
        state = newState;
    }

    private bool IsTargetInRange(float radius)
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance <= radius)
        {
            return true;
        }
        return false;
    }

    public bool IsDead()
    {
        return isDead;
    }

    private void WanderInTerrain()
    {
        agent.SetDestination(destination.transform.position);
    }

    public void HuntPlayer()
    {
        if (IsTargetInRange(lookRadius))
        {
            // enemy can see the target
            if (IsTargetInRange(shootRadius))
            {
                // enemy can shoot the target
                FaceTarget();
                SecondaryEnemyController.instance.FaceTarget();

                if (time >= 2f)
                {
                    time = 0f;
                    ShootTarget();
                }
            }
            else
            {
                // enemy can't shoot the target
                agent.SetDestination(player.position);
            }
        }
        else
        {
            // enemy can't see the target
            agent.SetDestination(destination.transform.position);
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void ShootTarget()
    {
        StartCoroutine(DelayShoot(delayTime));
    }

    IEnumerator DelayShoot(float delayTime)
    {
        //yield return new WaitForSeconds(delayTime);

        System.Random rnd = new System.Random();
        int hitRandNum = rnd.Next(0, hitRatio);

        yield return new WaitForSeconds(delayTime);

        if (!shootingSound.isPlaying)
        {
            shootingSound.Play();
        }
        if (hitRandNum == 1)
        {
            TargetHit target = p.GetComponent<TargetHit>();

            if (target != null)
            {
                target.TakeDamage(damage);
                Debug.Log(damage + " damage");
                SecondaryEnemyController.instance.ShootTarget(target, damage / 2);
            }
        }

        yield return new WaitForSeconds(delayTime);
    }

    private void SetAnimation()
    {
        //Store the newest vector at the end of the list of vectors
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            previousLocations[i] = previousLocations[i + 1];
        }
        previousLocations[previousLocations.Length - 1] = transform.position;

        //Check the distances between the points in your previous locations
        //If for the past several updates, there are no movements smaller than the threshold,
        //you can most likely assume that the object is not moving
        for (int i = 0; i < previousLocations.Length - 1; i++)
        {
            float distance = Vector3.Distance(previousLocations[i], previousLocations[i + 1]);

            if (distance >= noMovementThreshold)
            {
                //The minimum movement has been detected between frames
                if (hasGun)
                {
                    animator.SetInteger("States", (int)AnimationStates.PistolRunning);
                }
                else
                {
                    animator.SetInteger("States", (int)AnimationStates.Running);
                }

                isMoving = true;
                break;
            }
            else
            {
                if (hasGun)
                {
                    animator.SetInteger("States", (int)AnimationStates.PistolWalking);
                }
                else
                {
                    animator.SetInteger("States", (int)AnimationStates.Walking);
                }

                isMoving = false;
            }
        }
    }

    public bool IsMoving
    {
        get { return isMoving; }
    }

    public void EquipGun()
    {
        hasGun = true;
    }

    public void Stop()
    {
        stop = true;
        agent.enabled = false;
    }

    /*
private void OnDrawGizmosSelected()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, lookRadius);
}*/
}

public enum EnemyState 
{ 
    Wander, 
    RunAway, 
    Hunt, 
    Dead 
};


/*

        simple HUNTING algorithm:

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