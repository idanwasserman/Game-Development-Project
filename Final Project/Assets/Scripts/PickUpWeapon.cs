using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpWeapon : MonoBehaviour
{

    public GameObject gunInHand, gunInTerrain, gunTrigger;
    public AudioSource sound;
    public Text canvasText;

    private bool pickedUp = false;
    public static bool inPickUpArea = false; 


    private void Update()
    {
        if (!pickedUp && inPickUpArea)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    Debug.Log(hit.transform.name);
                    if (hit.transform.gameObject.name == "AmmoBox" && BoxMotion.isBoxOpen)
                    {
                        ActivateWeapon(true);
                    }
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (!pickedUp)
        {
            if (other.tag == "Weapon")
            {
                if (this.name == "MainEnemy")
                {
                    ActivateWeapon(false);
                }
            }
        }

    }

    // bool team refers to which team activate the weapon
    // true - player , false - enemy
    private void ActivateWeapon(bool team)
    {
        pickedUp = true;
        sound.Play();
        

        if (team)
        {
            GameManager.instance.UpdateGameState(GameState.PlayerAttacks);
            EnemyController.instance.UpdateEnemyState(EnemyState.RunAway);
            GunShooting.gunInPlayersHand = true;
            NPCAnimatorController.gs = GunState.Player;

        }
        else
        {
            GameManager.instance.UpdateGameState(GameState.PlayerDefends);
            EnemyController.instance.UpdateEnemyState(EnemyState.Hunt);
            NPCAnimatorController.gs = GunState.Enemy;
        }

        gunInHand.SetActive(true);
        gunInTerrain.SetActive(false);
        gunTrigger.SetActive(false);
        
        NPCAnimatorController.check = true;
    }

}
