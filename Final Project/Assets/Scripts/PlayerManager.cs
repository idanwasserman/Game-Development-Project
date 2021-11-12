using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    #region Singleton
    public static PlayerManager instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject player;

    private bool isDead = false;

    public bool IsDead()
    {
        return isDead;
    }
}
