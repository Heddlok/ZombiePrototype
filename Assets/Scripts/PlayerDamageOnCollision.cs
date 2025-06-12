using UnityEngine;

/// <summary>
/// Applies damage to the player when colliding with zombies via Rigidbody collisions.
/// </summary>
[RequireComponent(typeof(Collider), typeof(PlayerManager))]
public class PlayerDamageOnCollision : MonoBehaviour
{
    [Tooltip("Amount of damage to take when colliding with a Zombie")]
    [SerializeField]
    private int damageFromZombie = 10;

    [Tooltip("Minimum seconds between taking damage from zombies")]
    [SerializeField]
    private float damageCooldown = 3f;

    // Timestamp of the last time damage was applied
    private float _lastDamageTime = -Mathf.Infinity;

    private PlayerManager pm;

    private void Awake()
    {
        // Cache the PlayerManager component
        pm = GetComponent<PlayerManager>();
    }

    // This runs every FixedUpdate while the zombie’s trigger overlaps us
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Zombie") &&
            Time.time >= _lastDamageTime + damageCooldown)
        {
            if (pm != null)
                pm.TakeDamage(damageFromZombie);
            else
                Debug.LogError("PlayerManager component missing on Player!");

            _lastDamageTime = Time.time;
        }
    }
}
