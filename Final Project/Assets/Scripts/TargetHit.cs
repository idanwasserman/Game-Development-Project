using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
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
       // gameObject.SetActive(false);
       
        if (gameObject.name == "Player")
            GameManager.instance.GameOver();
        else
        {
            NPCAnimatorController.nameToKill = gameObject.name;
            NPCAnimatorController.toKill = true;
            if(name == "MainEnemy")
            {
                EnemyController.instance.UpdateEnemyState(EnemyState.Dead);
            }
        }
        // dead animation instead
    }
}
