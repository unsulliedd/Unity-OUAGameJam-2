using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
  public Transform bulletSpawnPoint;
  public GameObject bulletPrefab;
  public float bulletSpeed = 60;
  private AudioSource shootingSound;

  private void Start()
  {
   // shootingSound = GetComponent<AudioSource>();
  }


  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
      //shootingSound.Play();
      var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
      bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
    }
  }
}
