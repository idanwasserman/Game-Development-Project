using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxMotion : MonoBehaviour
{

    public Animator animator;
    public Text sideText;
    public AudioSource openSound, closeSound;

    public static bool isBoxOpen = false;
    private bool triggered = false;


    // Update is called once per frame
    void Update()
    {
        if (triggered)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isBoxOpen)
                {
                    //closeSound.Play(); // or playDelayed() if needed
                }
                else
                {
                    //openSound.Play(); // or playDelayed() if needed
                }
                Debug.Log("E pressed");
                isBoxOpen = !isBoxOpen;
                animator.SetBool("BoxIsOpenning", isBoxOpen);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            triggered = true;
            sideText.gameObject.SetActive(true);

            Debug.Log(name);
            if (name == "AmmoBoxController")
            {
                PickUpWeapon.inPickUpArea = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            triggered = false;
            sideText.gameObject.SetActive(false);

            if (name == "AmmoBox")
            {
                PickUpWeapon.inPickUpArea = false;
            }
        }


    }


}
