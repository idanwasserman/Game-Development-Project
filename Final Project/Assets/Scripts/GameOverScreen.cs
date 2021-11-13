using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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



    public void MainMenuButton()
    {

        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
