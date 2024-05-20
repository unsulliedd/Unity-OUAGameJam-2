using System;
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 60f;
    [SerializeField] private AudioClip shootingSound;
    [SerializeField] private int ammoCapacity = 60;
    [SerializeField] private float reloadTime = 1f;

    [Header("References")]
    private AudioSource audioSource;
    private Player player;
    private UIManager uiManager;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void Start()
    {
        player = GetComponentInParent<Player>();
        audioSource.clip = shootingSound;

        player.controls.Player.Fire.performed += ctx => Shoot();
        player.controls.Player.Reload.performed += ctx => Reload();
    }


    private void Shoot()
    {
        if (PlayerAnimation.Instance.CheckReloadAnimPlaying())
            return;
            
        audioSource.Play();
        PlayerAnimation.Instance.FireAnim();

        if (ammoCapacity == 0)
            player.controls.Player.Fire.Disable();
        else
            ammoCapacity--;

        uiManager.UpdateAmmoText(ammoCapacity);

        GameObject bullet = ObjectPool.Instance.GetBullet();
        bullet.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;

    }
    private void Reload()
    {
        if (ammoCapacity == 60)
            return;

        PlayerAnimation.Instance.ReloadAnim();
        ammoCapacity = 60;
        uiManager.UpdateAmmoText(ammoCapacity);
    }
}
