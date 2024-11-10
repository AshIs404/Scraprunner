using UnityEngine;

public class MachineGun : Weapon
{
    public GameObject bulletPrefab;    // The bullet prefab to instantiate
    public Transform firePoint;        // The point where the bullets are spawned
    public float rotationSpeed = 5f;   // How fast the gun rotates
    public float maxRotationAngle = 90f; // Maximum angle the gun can rotate from its initial direction

    private float targetAngle;         // The angle the gun should rotate to
    private float currentAngle;        // The current angle of the gun
    private Transform closestEnemy;    // The closest enemy
    private float initialAngle;        // The initial angle of the gun

    private Camera mainCamera;         // Reference to the main camera

    void Start()
    {
        initialAngle = transform.eulerAngles.z;  // Store the initial rotation angle
        currentAngle = initialAngle;             // Initialize current angle based on gun's initial rotation
        mainCamera = Camera.main;                // Automatically assign the main camera
    }

    void Update()
    {
        FindClosestEnemy();
        RotateGun();
        UpdateWeapon();  // This method will be called from the base class Weapon to handle firing
    }

    void FindClosestEnemy()
    {
        // Calculate the visible area of the camera
        Vector3 cameraPosition = mainCamera.transform.position;
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        float targetSearchRadius = Mathf.Max(cameraWidth, cameraHeight) / 2f;

        // Find all enemies within the visible area
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(cameraPosition, targetSearchRadius);

        float closestDistance = Mathf.Infinity;
        Transform closestTarget = null;

        foreach (Collider2D hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestTarget = enemy.transform;
                }
            }
        }

        // Set the closest enemy as the target
        closestEnemy = closestTarget;
    }

    void RotateGun()
    {
        float desiredAngle;

        // Find the closest enemy that is within the gun's rotation bounds
        Transform bestTarget = FindBestTargetInBounds();

        if (bestTarget != null)
        {
            // Determine the direction to the best target
            Vector2 direction = (bestTarget.position - transform.position).normalized;

            // Convert direction to angle in degrees
            targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // Get the gun's initial rotation relative to the car
            float gunInitialAngle = initialAngle + transform.parent.eulerAngles.z;

            // Calculate the delta angle between the current gun angle and the target angle
            float deltaAngle = Mathf.DeltaAngle(gunInitialAngle, targetAngle);

            // Clamp the delta angle to the allowed range
            deltaAngle = Mathf.Clamp(deltaAngle, -maxRotationAngle, maxRotationAngle);

            // Calculate the desired angle by applying the clamped delta to the initial gun angle
            desiredAngle = gunInitialAngle + deltaAngle;
        }
        else
        {
            // If no target is in range, return to the initial angle relative to the car
            desiredAngle = initialAngle + transform.parent.eulerAngles.z;
        }

        // Smoothly rotate the gun to the desired angle
        float currentAngle = Mathf.LerpAngle(transform.eulerAngles.z, desiredAngle, Time.deltaTime * rotationSpeed);

        // Apply the rotation to the gun
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    Transform FindBestTargetInBounds()
    {
        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;

        // Calculate the gun's initial angle in world space
        float gunInitialAngle = initialAngle + transform.parent.eulerAngles.z;

        // Calculate the allowable angle range
        float lowerBound = gunInitialAngle - maxRotationAngle;
        float upperBound = gunInitialAngle + maxRotationAngle;

        // Iterate through all enemies to find the best target within bounds
        foreach (Collider2D hitCollider in Physics2D.OverlapCircleAll(transform.position, 100f))
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Determine the direction to the enemy
                Vector2 direction = (enemy.transform.position - transform.position).normalized;

                // Calculate the angle to the enemy
                float angleToEnemy = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

                // Calculate the delta angle between the gun's initial angle and the target angle
                float deltaAngle = Mathf.DeltaAngle(gunInitialAngle, angleToEnemy);

                // Check if the angle is within the bounds
                if (deltaAngle >= -maxRotationAngle && deltaAngle <= maxRotationAngle)
                {
                    // Calculate distance to the enemy
                    float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

                    // If this enemy is closer than the previous best, update the best target
                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        bestTarget = enemy.transform;
                    }
                }
            }
        }

        return bestTarget;
    }



    public override void Fire()
    {
        if (closestEnemy == null) return;

        // Instantiate and fire the bullet in the direction the gun is currently facing
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = transform.up * bullet.GetComponent<Projectile>().speed;

        Debug.Log("MachineGun fired!");
    }
}
