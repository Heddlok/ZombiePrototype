using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Collider))]
public class Zombie : MonoBehaviour
{
    [Tooltip("Transform of the player to chase. Will auto-find by tag if left blank.")]
    public Transform targetTransform;

    [Tooltip("Damage dealt to the player on collision")]
    public int damageFromZombie = 10;

    [Tooltip("Zombie's starting health (unused here, but kept for later)")]
    public int health = 100;

    private NavMeshAgent agent;
    private PlayerManager playerManager;

    private void Awake()
    {
        // Cache the NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Disable Rigidbody physics to let NavMeshAgent control movement
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Auto-find the player if none assigned
        if (targetTransform == null)
        {
            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                targetTransform = playerObj.transform;
            else
                Debug.LogError($"{name}: No GameObject tagged 'Player' found.");
        }

        // Cache PlayerManager for damage calls
        if (targetTransform != null)
        {
            playerManager = targetTransform.GetComponent<PlayerManager>();
            if (playerManager == null)
                Debug.LogError($"{name}: PlayerManager component missing on Player!");
        }
    }

    private void Update()
    {
        if (targetTransform == null) return;

        // Tell the NavMeshAgent to move toward the player’s current position
        agent.SetDestination(targetTransform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Deal damage when we physically collide with the Player
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerManager != null)
                playerManager.TakeDamage(damageFromZombie);
            else
                Debug.LogError($"{name}: Can't deal damage—PlayerManager is null!");
        }
    }
}
