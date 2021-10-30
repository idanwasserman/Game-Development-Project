using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{

    public Text resultText;

    public void Setup(bool winningTeam) // winningTeam: true - Player, false - Enemy
    {
        gameObject.SetActive(true);
        if (winningTeam)
        {
            resultText.text = "Victory!";
        }
        else
        {
            resultText.text = "Defeat!";
        }
    }

    public void RestartButton()
    {
        GameManager.instance.RestartGame();
    }

    public void MainMenuButton()
    {

    }
}
