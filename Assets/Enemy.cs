using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageToCar = 20;  // Amount of damage the enemy deals to the car on collision
    public int maxHealth = 50;    // Health of the enemy
    private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the enemy collided with the car
        CarController car = collision.gameObject.GetComponent<CarController>();

        if (car != null)
        {
            // Deal damage to the car
            car.TakeDamage(damageToCar);

            // Destroy the enemy
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destroy the enemy game object
        Destroy(gameObject);
    }
}
