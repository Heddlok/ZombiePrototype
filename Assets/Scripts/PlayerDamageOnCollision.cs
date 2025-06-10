using UnityEngine;

/// <summary>
/// Applies damage to the player when colliding with zombies via Rigidbody collisions.
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerDamageOnCollision : MonoBehaviour
{
    [Tooltip("Amount of damage to take when colliding with a Zombie")]
    [SerializeField]
    private int damageFromZombie = 10;

    private PlayerManager pm;

    private void Awake()
    {
        // Cache the PlayerManager component
        pm = GetComponent<PlayerManager>();

        // Ensure this Rigidbody will send collision callbacks
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        // If you don’t want physics forces to move the player, you can freeze:
        // rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only handle collisions with objects tagged "Zombie"
        if (collision.GetComponent<Collider>().CompareTag("Zombie"))
        {
            if (pm != null)
                pm.TakeDamage(damageFromZombie);
            else
                Debug.LogError("PlayerManager component missing on Player!");
        }
    }
}
