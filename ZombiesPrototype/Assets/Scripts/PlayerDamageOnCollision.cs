using UnityEngine;

/// <summary>
/// Applies damage to the player when colliding with zombies via Rigidbody collisions.
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerDamageOnCollision : MonoBehaviour
{
    [Tooltip("Damage to apply when colliding with a Zombie")]
    [SerializeField]
    private int damageFromZombie = 10;

    private PlayerManager pm;

    private void Awake()
    {
        // Cache the PlayerManager component
        pm = GetComponent<PlayerManager>();

        // Ensure this Rigidbody will generate collision events
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        // If you don’t want physics forces to move the player, you can freeze constraints:
        // rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    /// <summary>
    /// Called by Unity when this Rigidbody collides with another collider.
    /// </summary>
    /// <param name="collision">Collision data from Unity</param>
    private void OnCollisionEnter(Collision collision)
    {
        // Only handle collisions with objects tagged "Zombie"
        if (collision.collider.CompareTag("Zombie") && pm != null)
        {
            pm.TakeDamage(damageFromZombie);
        }
    }
}
