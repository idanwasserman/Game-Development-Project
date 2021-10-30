using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    public GameState state;
    private float playerTeamCount = 2, enemiesCount = 2;
    public Text canvasText;
    public Image textBackground;
    public GameOverScreen gameOverScreen;


    private void Start()
    {
        state = GameState.Search;   
    }

    public void RestartGame()
    {
        state = GameState.Search;
        playerTeamCount = 2;
        enemiesCount = 2;
    }


    public void CharacterKilled(bool isEnemy)
    {
        if (isEnemy)
        {
            enemiesCount--;
            canvasText.text = "Enemy Died";
        }
        else
        {
            playerTeamCount--;
            canvasText.text = "Player Died";
        }

        if (playerTeamCount == 0 || enemiesCount == 0)
        {
            UpdateGameState(GameState.GameOver);
        }
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.Search:
                HandleSearch();
                break;

            case GameState.PlayerAttacks:
                HandlePlayerAttacks();
                break;

            case GameState.PlayerDefends:
                HandlePlayerDefends();
                break;

            case GameState.GameOver:
                GameOver();
                break;

            default:
                break;
        }
    }

    private void HandleSearch()
    {
        EnemyController.instance.UpdateEnemyState(EnemyState.Wander);
        canvasText.text = "Find a Weapon";
        textBackground.color = new Color(0, 0, 0xff, textBackground.color.a);
        
    }

    private void HandlePlayerAttacks()
    {
        EnemyController.instance.UpdateEnemyState(EnemyState.RunAway);
        canvasText.text = "Hunt the Enemies";
        textBackground.color = new Color(0, 0xff, 0, textBackground.color.a);
    }

    private void HandlePlayerDefends()
    {
        EnemyController.instance.UpdateEnemyState(EnemyState.Hunt);
        canvasText.text = "Run Away from the Enemies";
        textBackground.color = new Color(0xff, 0, 0, textBackground.color.a);
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");

        gameOverScreen.Setup(enemiesCount == 0);
    }
}

public enum GameState
{
    Search,
    PlayerAttacks,
    PlayerDefends,
    GameOver
}