using UnityEngine;

public class Missile : Projectile
{
    public float rotateSpeed = 200f;  // Rotation speed for homing
    private Transform target;         // The target the missile will home in on

    protected override void Start()
    {
        base.Start();

        // Find the target (for example, the nearest enemy)
        target = FindClosestEnemy();
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.linearVelocity = transform.up * speed;
        }
        else
        {
            // If no target, just move straight
            rb.linearVelocity = transform.up * speed;
        }
    }

    Transform FindClosestEnemy()
    {
        // This is a simple example. You should replace it with your actual enemy detection logic.
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length == 0)
        {
            return null;
        }

        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = enemy.transform;
            }
        }

        return closest;
    }
}
