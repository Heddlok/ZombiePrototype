using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public int damage = 25;

    void Start() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"Player hit! Health now {currentHealth}.");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Player died!");
        // TODO: game over / respawn logic
        
    }
}

