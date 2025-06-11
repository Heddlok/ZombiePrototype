using UnityEngine;

public class ZombieDeathNotifier : MonoBehaviour
{
    public ZombieSpawner spawner;

    void OnDestroy()
    {
        if (spawner != null)
            spawner.OnZombieKilled();
    }
}
