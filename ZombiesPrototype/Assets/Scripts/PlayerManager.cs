using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Maximum health of the player")]
    [SerializeField] private int maxHealth = 100;

    [Header("Regeneration Settings")]
    [Tooltip("Seconds without damage before regen starts")]
    [SerializeField] private float regenDelay = 3f;
    [Tooltip("Health points per second when regenerating")]
    [SerializeField] private float regenRate = 10f;

    private int currentHealth;
    private Coroutine regenCoroutine;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Applies damage, resets the regen timer, and (re)starts the regen coroutine.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (amount <= 0 || currentHealth <= 0)
            return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log($"Player took {amount} damage, health now {currentHealth}/{maxHealth}");

        if (currentHealth == 0)
        {
            Die();
            // Stop any pending regeneration
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
                regenCoroutine = null;
            }
        }
        else
        {
            // Restart the regen delay
            if (regenCoroutine != null)
                StopCoroutine(regenCoroutine);
            regenCoroutine = StartCoroutine(Regenerate());
        }
    }

    /// <summary>
    /// Waits regenDelay seconds, then heals 1 HP every (1/regenRate) seconds until full.
    /// </summary>
    private IEnumerator Regenerate()
    {
        yield return new WaitForSeconds(regenDelay);

        while (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + 1, maxHealth);
            Debug.Log($"Player regenerates to {currentHealth}/{maxHealth}");
            yield return new WaitForSeconds(1f / regenRate);
        }

        regenCoroutine = null;
    }

    private void Die()
    {
        Debug.Log("Player died!");
        gameObject.SetActive(false);
        Debug.Log("Player has been disabled due to death.");
    }
}
