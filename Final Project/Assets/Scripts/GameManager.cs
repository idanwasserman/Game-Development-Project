using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float enemiesCount = 2;
    float playerTeamCount = 2;

    public void gameOver()
    {
        Debug.Log("GAME OVER");
    }

    public void characterKilled(bool isEnemy)
    {
        if (isEnemy)
            enemiesCount--;
        else
            playerTeamCount--;

        if (enemiesCount == 0 || playerTeamCount == 0)
            gameOver();

    }
}
