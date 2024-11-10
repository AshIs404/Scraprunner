using UnityEngine;

public class Zombie : Enemy
{
    public float speed = 2f;
    private Transform player;
    public int damage = 10;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        CarController carController = hitInfo.GetComponent<CarController>();
        if (carController != null)
        {
            carController.TakeDamage(damage);
            Destroy(gameObject);  // Optionally destroy the zombie on collision
        }
    }
}
