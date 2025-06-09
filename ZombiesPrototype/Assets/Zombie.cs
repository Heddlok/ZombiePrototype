using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// AI controller for Zombie: detects player, chases, and deals damage on collision.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{
    [Header("Detection & Attack")]
    [Tooltip("Transform of the player to chase (assign via Inspector or set at runtime)")]
    public Transform playerTransform;

    [Tooltip("Damage dealt to player on collision")]
    public int damage = 10;

    [Tooltip("Cooldown in seconds between damage ticks")]
    public float damageCooldown = 1f;

    private NavMeshAgent agent;
    private float nextDamageTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (playerTransform == null)
        {
            var playerGO = GameObject.FindWithTag("Player");
            if (playerGO != null)
                playerTransform = playerGO.transform;
            else
                Debug.LogError("Player Transform not assigned and no GameObject tagged 'Player' found.");
        }
    }

    void Update()
    {
        if (playerTransform != null)
            agent.SetDestination(playerTransform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            var pm = collision.collider.GetComponent<PlayerManager>();
            if (pm != null)
                pm.TakeDamage(damage);

            nextDamageTime = Time.time + damageCooldown;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (playerTransform != null)
            Gizmos.DrawLine(transform.position, playerTransform.position);
    }
}