using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;       // The enemy prefab to spawn
    public float spawnInterval = 2f;   // Time between spawns
    public float spawnDistance = 1f;   // Distance outside the screen bounds where enemies will spawn
    public float playerSpeedThreshold = 10f; // Speed threshold for adaptive spawning

    private Camera mainCamera;         // Reference to the main camera
    private Transform player;          // Reference to the player (car)
    private float nextSpawnTime;

    void Start()
    {
        mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // Calculate screen bounds
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        float screenHalfHeight = mainCamera.orthographicSize;
        float screenHalfWidth = screenHalfHeight * mainCamera.aspect;

        // Calculate the direction based on player's movement
        Vector3 spawnDirection = GetSpawnDirectionBasedOnPlayerMovement();

        // Determine the spawn position just outside the screen bounds
        Vector3 spawnPosition = player.position + spawnDirection * (Mathf.Max(screenHalfWidth, screenHalfHeight) + spawnDistance);

        // Instantiate the enemy at the calculated spawn position
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    Vector3 GetSpawnDirectionBasedOnPlayerMovement()
    {
        if(player == null)
        {
            return Random.insideUnitCircle.normalized;
        }

        // Calculate player's velocity
        Vector3 playerVelocity = player.GetComponent<Rigidbody2D>().linearVelocity;

        // Normalize the velocity to get the movement direction
        Vector3 movementDirection = playerVelocity.normalized;

        // If player is moving fast enough, favor the movement direction
        if (playerVelocity.magnitude > playerSpeedThreshold)
        {
            return movementDirection;
        }
        else
        {
            // If the player is not moving fast enough, spawn randomly around the screen edges
            return Random.insideUnitCircle.normalized;
        }
    }
}