using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    float enemiesCount = 2;
    float playerTeamCount = 2;
    public Text canvasText;
    public void gameOver()
    {
        Debug.Log("GAME OVER");
        if(enemiesCount == 0)
            canvasText.text = "Game Over!!\nPlayer Wins!!";
        else
            canvasText.text = "Game Over!!\nEnemy Wins!!";

    }

    public void characterKilled(bool isEnemy)
    {
        if (isEnemy)
        {
            enemiesCount--;
            canvasText.text = "Enemy Die";
        }
        else
        {
            playerTeamCount--;
            canvasText.text = "Player Die";
        }
        if (enemiesCount == 0 || playerTeamCount == 0)
            gameOver();

    }
}
