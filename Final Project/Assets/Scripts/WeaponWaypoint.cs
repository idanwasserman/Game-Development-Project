using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWaypoint : MonoBehaviour
{
    public Image img;
    public Transform target;
    public Text meter;
    public Vector3 offset;
    public GameObject weapon;

    private float minX, minY, maxX, maxY;
    private bool isPointShown, weaponFound;


    // Start is called before the first frame update
    void Start()
    {
        isPointShown = false;
        weaponFound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(! weaponFound)
        {
            minX = img.GetPixelAdjustedRect().width / 2;
            maxX = Screen.width - minX;

            minY = img.GetPixelAdjustedRect().height / 2;
            maxY = Screen.height - minY;

            Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

            if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
            {
                // target is behind the player
                if (pos.x < Screen.width * 2)
                {
                    pos.x = maxX;
                }
                else
                {
                    pos.x = minX;
                }
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            img.transform.position = pos;
            meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";

            if (Input.GetKeyDown(KeyCode.H))
            {
                isPointShown = !isPointShown;

                img.gameObject.SetActive(isPointShown);
            }
        }

        if (! weapon.activeSelf)
        {
            img.gameObject.SetActive(false);
            weaponFound = true;
        }
    }

}
