using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public static int maxHealth = 100;
    private int currentHealth;
    public HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int amountDamage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= amountDamage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0f)
        {
            GameManager.instance.CharacterKilled(this.tag == "Enemy");

            Die();
        }
    }

    void Die()
    {      
        if (name == "Player")
        {
            GameManager.instance.GameOver();
        }
        else
        {
            if (name == "PlayerHelper")
            {
                HelperController.instance.Kill();
            }
            else if (name == "MainEnemy")
            {
                EnemyController.instance.Kill();
            }
            else if (name == "SecondaryEnemy")
            {
                SecondaryEnemyController.instance.Kill();
            }
        }
    }

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }
}
