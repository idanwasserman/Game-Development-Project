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
        currentHealth -= amountDamage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth <= 0f)
        {
            if (this.tag == "Player")
            {
                //FindObjectOfType<GameManager>().characterKilled(false);
                GameManager.enemiesCount--;
            }

            else
                FindObjectOfType<GameManager>().characterKilled(true);
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
