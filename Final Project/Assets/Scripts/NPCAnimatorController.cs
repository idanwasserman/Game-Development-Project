using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimatorController : MonoBehaviour
{
    private Animator animator;
    //private Transform objectTransfom;//Set this to the transform you want to check

    private float noMovementThreshold = 0.05f, walkingThreshold = 0.25f;
    private const int noMovementFrames = 3;
    Vector3[] previousLocations = new Vector3[noMovementFrames];
    private bool isMoving;
    public static GunState gs = GunState.None;
    public static bool check = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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

        if (check && gs != GunState.None)
        {
            if (gs == GunState.Player && tag == "Player")
            {
                animator.SetBool("hasGun", true);
            }
            else if (gs == GunState.Enemy && tag == "Enemy")
            {
                animator.SetBool("hasGun", true);
            }
            
            check = false;
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

            if (distance >= walkingThreshold)
            {
                //The minimum movement has been detected between frames
                animator.SetInteger("States", (int)States.Running);
                isMoving = true;
                break;
            }
            else if (distance >= noMovementThreshold)
            {
                animator.SetInteger("States", (int)States.Walking);
                isMoving = true;
            }
            else
            {
                animator.SetInteger("States", (int)States.Idle);
                isMoving = false;
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