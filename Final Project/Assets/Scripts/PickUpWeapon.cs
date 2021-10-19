using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpWeapon : MonoBehaviour
{

    public GameObject gunInHand, gunInTerrain, gunTrigger;
    public AudioSource sound;
    public Text canvasText;


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Weapon")
        {
            sound.Play();
            
            if (this.tag == "Player")
            {
                canvasText.text = "Hunt Down the Enemy";
                EnemyController.state = (int) EnemyController.EnemyState.RunAway;
            }
            else if (this.tag == "Enemy")
            {
                canvasText.text = "Run Away From Enemy";
                EnemyController.state = (int) EnemyController.EnemyState.Hunt;
            }

            gunInHand.SetActive(true);
            gunInTerrain.SetActive(false);
            gunTrigger.SetActive(false);
        }

    }

}
