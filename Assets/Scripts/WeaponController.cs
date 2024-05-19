using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 60f;
    [SerializeField] private AudioClip shootingSound;
    private AudioSource audioSource;
    private Player player;
    private Animator animator;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        player = GetComponentInParent<Player>();
        audioSource.clip = shootingSound;

        player.controls.Player.Fire.performed += ctx => Shoot();
    }

    private void Shoot()
    {
        audioSource.Play();
        animator.SetTrigger("Fire");

        GameObject bullet = ObjectPool.Instance.GetBullet();
        bullet.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
    }
}
