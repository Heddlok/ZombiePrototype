using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    [Tooltip("The Zombie prefab to spawn")]
    public GameObject zombiePrefab;

    [Tooltip("Transforms where zombies will appear")]
    public Transform[] spawnPoints;

    [Tooltip("Seconds between rounds")]
    public float timeBetweenRounds = 5f;

    private readonly List<GameObject> _spawnedZombies = new List<GameObject>();

    private int round = 1;
    private int zombiesToSpawn;
    private int zombiesAlive;
    private float zombieHealth = 50f;
    private bool spawningRound = false;
    private float roundTimer = 0f;

    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        // Remove any destroyed zombies from the list
        _spawnedZombies.RemoveAll(z => z == null);

        // If all zombies are dead and we're not already spawning a new round, start next round timer
        if (zombiesAlive == 0 && !spawningRound)
        {
            spawningRound = true;
            roundTimer = timeBetweenRounds;
        }

        if (spawningRound)
        {
            roundTimer -= Time.deltaTime;
            if (roundTimer <= 0f)
            {
                round++;
                zombieHealth *= 1.25f; // Increase health each round
                StartRound();
                spawningRound = false;
            }
        }
    }

    private void StartRound()
    {
        zombiesToSpawn = Mathf.RoundToInt(5 * Mathf.Pow(1.5f, round - 1));
        zombiesAlive = zombiesToSpawn;

        for (int i = 0; i < zombiesToSpawn; i++)
        {
            SpawnZombie();
        }

        Debug.Log($"Round {round}: Spawned {zombiesToSpawn} zombies (Health: {zombieHealth})");
    }

    private void SpawnZombie()
    {
        if (zombiePrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning($"[{name}] ZombieSpawner missing prefab or spawnPoints");
            return;
        }

        // Pick a random spawn point
        Transform pt = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject z = Instantiate(zombiePrefab, pt.position, pt.rotation);

        // Set zombie health for this round (assumes zombies have a public 'health' field)
        var target = z.GetComponent<Target>();
        if (target != null)
            target.health = zombieHealth;

        // Register kill callback
        var notifier = z.AddComponent<ZombieDeathNotifier>();
        notifier.spawner = this;

        _spawnedZombies.Add(z);
    }

    // Called by ZombieDeathNotifier when a zombie dies
    public void OnZombieKilled()
    {
        zombiesAlive--;
    }
}
