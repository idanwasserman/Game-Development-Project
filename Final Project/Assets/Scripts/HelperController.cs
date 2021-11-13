using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelperController : MonoBehaviour
{
    #region Singleton
    public static HelperController instance;
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

    public float delayTime = 1.5f;
    private bool isDead = false;
    public int hitRatio = 10;

    // shooting
    private AudioSource gunSound;

    // animations variables
    private Animator animator;
    public float noMovementThreshold = 0.005f;
    private const int noMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    private bool isMoving;
    private bool hasGun = false;

    // follow player variables
    private NavMeshAgent agent;
    public Transform target;
    //public GameObject destination;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        gunSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        SetAnimation();

        if (!PlayerManager.instance.IsDead())
        {
            FollowTarget();
        }
        else
        {
            //agent.SetDestination(destination.transform.position);
        }
        
    }

    public void ShootTarget(TargetHit target, int damage)
    {
        StartCoroutine(DelayShoot(delayTime, target, damage));
    }

    IEnumerator DelayShoot(float delayTime, TargetHit target, int damage)
    {
        yield return new WaitForSeconds(delayTime);

        System.Random rnd = new System.Random();
        int hitRandNum = rnd.Next(0, hitRatio);

        if (!gunSound.isPlaying)
        {
            gunSound.Play();
        }

        if (hitRandNum == 1)
        {
            target.TakeDamage(damage);
        }
    }

    private void FollowTarget()
    {
        float x, y, z;
        x = target.position.x;
        z = target.position.z;
        y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));
        Vector3 newPosition = new Vector3(x, y, z);
        agent.SetDestination(newPosition);
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

    public bool IsDead()
    {
        return isDead;
    }

    public void Kill()
    {
        isDead = true;
        agent.enabled = false;
        animator.SetInteger("States", (int)AnimationStates.Dead);
    }

    public void EquipGun()
    {
        hasGun = true;
    }
}

