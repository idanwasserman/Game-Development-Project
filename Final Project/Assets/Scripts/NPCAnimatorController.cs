using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimatorController : MonoBehaviour
{
    private Animator animator;
    private float noMovementThreshold = 0.025f;
    private const int noMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    private bool isMoving;

    public static GunState gs = GunState.None;
    public static bool check = false;
    public static string nameToKill;
    public static bool toKill = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        nameToKill = "";
    }

    public bool IsMoving
    {
        get { return isMoving; }
    }

    void Awake()
    {
        //For good measure, set the previous locations
        for (int i = 0; i < previousLocations.Length; i++)
        {
            previousLocations[i] = Vector3.zero;
        }
    }

    void Update()
    {

        if (gs != GunState.None && check)
        { 
            Debug.Log("IN Check " + gs + " " + tag);
            if (gs == GunState.Player && tag == "Player")
            {
                animator.SetBool("hasGun", true);

            }
            else if (gs == GunState.Enemy && tag == "Enemy")
            {
                animator.SetBool("hasGun", true);
            }
            
        }


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
                animator.SetInteger("States", (int)AnimationStates.Running);
                isMoving = true;
                break;
            }
            else
            {
                animator.SetInteger("States", (int)AnimationStates.Walking);
                isMoving = false;
            }
        }

        if (toKill)
        {
            if (name == nameToKill)
            {
                animator.SetBool("isDying", true);
                toKill = false;
            }
        }
    }
}

public enum States
{
    Idle,
    Walking,
    Running
}

public enum GunState
{
    None,
    Player,
    Enemy
}