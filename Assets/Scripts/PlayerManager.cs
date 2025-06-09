using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"Player took {amount} damage, now at {currentHealth}.");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Player died!");
        // your respawn or game over logic here
    }
}
