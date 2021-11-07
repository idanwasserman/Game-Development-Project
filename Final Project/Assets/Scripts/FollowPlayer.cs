using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{

    public Transform target;
    NavMeshAgent agent;
    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float x, y, z;
        x = target.position.x;
        z = target.position.z;
        y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));
        Vector3 newPosition = new Vector3(x, y, z);
        agent.SetDestination(newPosition);
        //FaceTarget();
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
