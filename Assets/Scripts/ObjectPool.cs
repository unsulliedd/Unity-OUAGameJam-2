using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Bullet Pool")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletPoolSize = 60;
    private Queue<GameObject> bulletPool;

    [Header("Enemy Pool")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyPoolSize = 20;
    private Queue<GameObject> enemyPool;

    public static ObjectPool Instance { get; private set; }

    void Awake()
    {
        // Set the Instance if it's not already set
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        bulletPool = new Queue<GameObject>();
        enemyPool = new Queue<GameObject>();
        CreateInitialBulletPool();
        CreateInitialEnemyPool();
    }

    private void CreateInitialBulletPool()
    {
        for (int i = 0; i < bulletPoolSize; i++)
            CreateBullet();
    }

    private void CreateInitialEnemyPool()
    {
        for (int i = 0; i < enemyPoolSize; i++)
            CreateEnemy();
    }

    private void CreateBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        newBullet.transform.parent = this.transform;
        bulletPool.Enqueue(newBullet);
    }

    private void CreateEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.SetActive(false);
        newEnemy.transform.parent = this.transform;
        enemyPool.Enqueue(newEnemy);
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0)
            CreateBullet();

        GameObject bulletToGet = bulletPool.Dequeue();
        bulletToGet.SetActive(true);
        return bulletToGet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.parent = this.transform;
        bulletPool.Enqueue(bullet);
    }

    public GameObject GetEnemy()
    {
        if (enemyPool.Count == 0)
            CreateEnemy();

        GameObject enemyToGet = enemyPool.Dequeue();
        enemyToGet.SetActive(true);
        return enemyToGet;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemy.transform.parent = this.transform;
        enemyPool.Enqueue(enemy);
    }
}
