using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 100;
    [SerializeField] private GameObject explosionFx;

    public void TakeDamage(int damage)
    {
        if (health >= 0)
            health -= damage;
        else
        {
            Destroy(gameObject);
            ExplosionFx();
        }
    }

    private void ExplosionFx()
    {
        GameObject explosionImpactFx = Instantiate(explosionFx, transform.position, Quaternion.identity);
        Destroy(explosionImpactFx, 1f);
    }
}
