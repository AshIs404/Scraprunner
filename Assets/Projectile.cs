using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float speed = 30f;            // Speed of the projectile
    public int damage = 10;              // Damage dealt by the projectile
    public Rigidbody2D rb;               // Rigidbody2D component of the projectile

    protected virtual void Start()
    {
        // Ensure the Rigidbody2D component is attached
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Disable gravity by setting gravityScale to 0
        rb.gravityScale = 0;

        // Set the bullet's velocity to move in a straight line
        rb.linearVelocity = transform.up * speed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check if the projectile hit an enemy
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Destroy the projectile after it hits something
        Destroy(gameObject);
    }
}
