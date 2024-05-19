using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Bullet Pool")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletPoolSize = 60;
    private Queue<GameObject> bulletPool;
   
    public static ObjectPool Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        bulletPool = new Queue<GameObject>();
        CreateInitialBulletPool(); 
    }

    private void CreateInitialBulletPool()
    {
        for (int i = 0; i < bulletPoolSize; i++)
            CreateBullet();
    }

    private void CreateBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        newBullet.transform.parent = this.transform;
        bulletPool.Enqueue(newBullet);
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count == 1)
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
}
