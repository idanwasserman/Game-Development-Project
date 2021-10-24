using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DestinationChange : MonoBehaviour
{
    public float minX, maxX, minZ, maxZ;
    private float x, y, z;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            x = Random.Range(minX, maxX);
            z = Random.Range(minZ, maxZ);

            y = Terrain.activeTerrain.SampleHeight(new Vector3(x, 0, z));

            transform.position = new Vector3(x, y, z);
            Debug.Log("new dest: " + transform.position);
        }
    }
}
