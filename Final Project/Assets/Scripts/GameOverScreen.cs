using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        PickUpWeapon.SetPickedUp(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuButton()
    {
        // TODO or delete
    }
}
