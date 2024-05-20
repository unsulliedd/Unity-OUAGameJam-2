using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int initialEnemyCount = 20; // Starting number of enemies

    private ObjectPool enemyPool;
    [SerializeField] private GameObject groundPlane;

    private void Start()
    {
        // Get reference to the enemy object pool
        enemyPool = ObjectPool.Instance;

        // Spawn initial enemies
        SpawnInitialEnemies();
    }

    private void SpawnInitialEnemies()
    {
        // Spawn initial number of enemies
        for (int i = 0; i < initialEnemyCount; i++)
        {
            GameObject enemy = enemyPool.GetEnemy();
            // Randomize spawn position on the ground plane
            Vector3 randomPosition = GetRandomPositionOnGround();
            enemy.transform.position = randomPosition;
        }
    }

    private Vector3 GetRandomPositionOnGround()
    {
        // Get the size of the ground plane
        Vector3 groundSize = groundPlane.GetComponent<Renderer>().bounds.size;

        // Calculate random position within the ground plane bounds
        float randomX = Random.Range(-groundSize.x / 2f, groundSize.x / 2f);
        float randomZ = Random.Range(-groundSize.z / 2f, groundSize.z / 2f);

        // Ensure enemies spawn at ground level
        Vector3 randomPosition = new Vector3(randomX, 0f, randomZ);

        // Offset the position to the actual position of the ground plane
        randomPosition += groundPlane.transform.position;

        return randomPosition;
    }
}
