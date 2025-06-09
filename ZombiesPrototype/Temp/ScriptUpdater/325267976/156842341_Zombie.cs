using UnityEngine;
public class Zombie : MonoBehaviour
{
   
    public Transform targetTransform;
    public Transform zombiesTransform;
    public GameObject player;
    public PlayerManager playerManager; 

    public int damageFromZombie;

    public Vector3 pos;
    public int health = 100;
    public float speed = 5f;
    void OnCollisionEnter(Collision collision)
{
        if (collision.GetComponent<Collider>().CompareTag("Zombie"))
        {
            playerManager.TakeDamage(damageFromZombie);
       
    }
}





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
            {
                Vector3 pos = player.transform.position;
                Debug.Log($"Found it at {pos}");
            }
        else
        {
        Debug.LogWarning("Could not find that object!");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTransform == null) return;
        Vector3 playerToDistance = pos - transform.position;
        transform.TransformDirection(pos);
        Debug.Log($"Players Position{pos}");
        Debug.Log($"Distance {playerToDistance}");
        
        Vector3 direction = (pos - transform.position).normalized;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
                   

        // 3) Move yourself along that direction
        transform.position += direction * speed * Time.deltaTime;
      
        

    }
}
