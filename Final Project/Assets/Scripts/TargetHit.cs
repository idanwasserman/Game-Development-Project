
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float amountDamage)
    {
        health -= amountDamage;
        if(health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
