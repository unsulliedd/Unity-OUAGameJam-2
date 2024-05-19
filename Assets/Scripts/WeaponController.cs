using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 60f;
    [SerializeField] private AudioClip shootingSound;
    private AudioSource audioSource;
    private Player player;
    private Animator animator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<Animator>();

        if (bulletSpawnPoint == null || bulletPrefab == null)
        {
            Debug.LogError("Bullet spawn point or bullet prefab is not set.");
        }
    }

    private void Start()
    {
        player = GetComponentInParent<Player>();
        audioSource.clip = shootingSound;

        if (player != null)
        {
            player.controls.Player.Fire.performed += ctx => Shoot();
        }
        else
        {
            Debug.LogError("Player component is not found.");
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.controls.Player.Fire.performed -= ctx => Shoot();
        }
        else
            Debug.LogError("Player component is not found.");
    }

    private void Shoot()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            audioSource.Play();
            animator.SetTrigger("Fire");

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        }
        else
        {
            Debug.LogError("Bullet prefab or bullet spawn point is missing.");
        }
    }
}
