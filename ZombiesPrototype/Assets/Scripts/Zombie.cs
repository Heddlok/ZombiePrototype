using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Collider), typeof(Rigidbody))]
public class Zombie : MonoBehaviour
{
    [Tooltip("Transform of the player to chase. Will auto-find by tag if left blank.")]
    public Transform targetTransform;

    [Tooltip("Damage dealt to the player on collision")]
    public int damageFromZombie = 10;

    [Tooltip("Zombie's starting health")]
    public int health = 50;

    [Tooltip("Points per bullet hit")]
    public int pointsPerBulletHit = 10;

    [Tooltip("Points per kill")]
    public int pointsPerKill = 120;

    private NavMeshAgent agent;
    private PlayerManager playerManager;

    private void Awake()
    {
        // Cache components
        agent = GetComponent<NavMeshAgent>();
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;   // NavMeshAgent drives movement
        rb.useGravity   = false;

        // Find & cache the player
        if (targetTransform == null)
        {
            var playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                targetTransform = playerObj.transform;
            else
                Debug.LogError($"{name}: No GameObject tagged 'Player' found.");
        }

        if (targetTransform != null)
        {
            playerManager = targetTransform.GetComponent<PlayerManager>();
            if (playerManager == null)
                Debug.LogError($"{name}: PlayerManager component missing on Player!");
        }

        // Make sure our collider is a trigger so OnTriggerEnter fires
        GetComponent<Collider>().isTrigger = true;
    }

    private void Update()
    {
        if (targetTransform != null)
            agent.SetDestination(targetTransform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Hurt the player on contact
        if (other.CompareTag("Player"))
        {
            if (playerManager != null)
                playerManager.TakeDamage(damageFromZombie);
            else
                Debug.LogError($"{name}: Can't deal damage—PlayerManager is null!");
        }
    }

    /// <summary>
    /// Call this when your gun raycast or projectile hits the zombie.
    /// </summary>
    /// <param name="damage">Amount of health to remove.</param>
    public void TakeDamage(int damage)
    {
        // Award points for the hit
        ScoreManager.Instance.AddPoints(pointsPerBulletHit);

        // Apply damage
        health -= damage;

        // If dead, award kill points and destroy
        if (health <= 0)
        {
            ScoreManager.Instance.AddPoints(pointsPerKill);
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} has died.");
        Destroy(gameObject);
    }
}
