using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public int damage = 10;
    public float range = 100f;

    public Camera fpsCam;
    public AudioSource gunSound;

    public static bool gunInPlayersHand = false;


    // Update is called once per frame
    void Update()
    {   
        if (gunInPlayersHand)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                gunSound.Play();
                Shoot();
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            TargetHit target = hit.transform.GetComponent<TargetHit>();

            if(target != null)
            {
                target.TakeDamage(damage);

                HelperController.instance.ShootTarget(target, damage / 2);
            }
        }
    }
}
