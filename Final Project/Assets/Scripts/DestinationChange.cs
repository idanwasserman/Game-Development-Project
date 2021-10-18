using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestinationChange : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "MainEnemy")
        {
            bool isDestWalkable = false;

            while(! isDestWalkable)
            {
                float x, y, z;

                x = Random.Range(0, 1000);
                z = Random.Range(0, 1000);

                y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));

                Vector3 newDestination = new Vector3(x, y, z);
                NavMeshHit hit;
                if (NavMesh.SamplePosition(newDestination, out hit, 1f, NavMesh.AllAreas))
                {
                    this.gameObject.transform.position = new Vector3(x, y, z);
                    isDestWalkable = true;
                    Debug.Log("reachable");
                }
                else
                {
                    Debug.Log("not reachable");
                }
            }

        }
    }

}
