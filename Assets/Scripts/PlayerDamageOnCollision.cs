using UnityEngine;
public class PlayerDamageOnCollision : MonoBehaviour
{

    public int damageFromZombie = 50;

    public PlayerManager pm;
    void Awake()
    {
        // Cache the PlayerManager so we don’t call GetComponent<> every time
        pm = GetComponent<PlayerManager>();
    }

    void Start()
    {
    
   
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.GetComponent<Collider>().CompareTag("Zombie") && pm != null)
        {
            // Directly call TakeDamage on your PlayerManager
            pm.TakeDamage(damageFromZombie);
        }
    }
}
