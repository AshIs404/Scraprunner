using UnityEngine;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public float forwardSpeed = 10f;
    public float reverseSpeed = 4f;
    public float acceleration = 2f;
    public float maxSpeed = 15f;
    public float maxTurnAngle = 45f;
    public float turnSpeed = 100f;

    public int maxHealth = 100;         // Maximum health of the car
    private int currentHealth;          // Current health of the car
    public List<Weapon> weapons;        // List of weapons attached to the car

    public float currentSpeed = 0f;
    private float currentTurnAngle = 0f;

    private Rigidbody2D rb;             // Reference to the Rigidbody2D component

    void Start()
    {
        // Initialize the car's health
        currentHealth = maxHealth;

        // Get the Rigidbody2D component attached to the car
        rb = GetComponent<Rigidbody2D>();

        // Ensure the Rigidbody2D exists
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from this game object");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleTurning();
        HandleWeapons();
    }

    void HandleMovement()
    {
        float moveDirection = Input.GetAxis("Vertical");

        if (moveDirection > 0)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        }
        else if (moveDirection < 0)
        {
            currentSpeed -= acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -reverseSpeed, 0);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, acceleration * Time.deltaTime);
        }

        // Apply velocity to the Rigidbody2D for movement
        rb.linearVelocity = transform.up * currentSpeed;
    }

    void HandleTurning()
    {
        float turnDirection = Input.GetAxis("Horizontal");

        if (turnDirection != 0)
        {
            currentTurnAngle = turnDirection * maxTurnAngle * (currentSpeed / maxSpeed);
            currentTurnAngle = Mathf.Clamp(currentTurnAngle, -maxTurnAngle, maxTurnAngle);

            // Apply rotation to the Rigidbody2D for turning
            float rotationAmount = -currentTurnAngle * turnSpeed * Time.deltaTime;
            rb.MoveRotation(rb.rotation + rotationAmount);
        }
    }

    void HandleWeapons()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.UpdateWeapon();  // Call the Update method for each weapon to handle firing
        }
    }

    // Method to deal damage to the car
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Car took " + damage + " damage!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle the car's death
    void Die()
    {
        Debug.Log("Car destroyed!");
        // Handle the car's destruction (e.g., play explosion animation, end game, etc.)
        Destroy(gameObject);
    }

}
