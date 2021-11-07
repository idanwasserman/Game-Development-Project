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
        gameObject.SetActive(false);
        // dead animation instead
    }
}
