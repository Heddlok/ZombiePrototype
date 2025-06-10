using UnityEngine;

/// <summary>
/// Manages player health, damage, and death.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Maximum health of the player")]
    [SerializeField]
    private int maxHealth = 100;

    private int currentHealth;

    private void Awake()
    {
        // Initialize health
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Applies damage to the player. Clamps health to zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (amount <= 0 || currentHealth <= 0)
            return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log($"Player took {amount} damage, health now {currentHealth}/{maxHealth}");

        if (currentHealth == 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: game over or respawn logic here
    }
}
