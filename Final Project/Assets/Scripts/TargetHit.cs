using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float amountDamage)
    {
        health -= amountDamage;
        if(health <= 0f)
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
