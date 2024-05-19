using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;

    void Update()
    {
        // Return after 7 seconds if it doesn't hit an anything
        StartCoroutine(ReturnToPoolAfterDelay(7f)); 
    }

    private void OnCollisionEnter(Collision other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(10);

        CreateHitImpactFx(other);

        if (other.gameObject.CompareTag("Enemy"))
        {
            // Enemy damage logic 
            ObjectPool.Instance.ReturnBullet(gameObject);
        }
        else
            StartCoroutine(ReturnToPoolAfterDelay(3f));
    }

    private void CreateHitImpactFx(Collision other)
    {
        if (other.contacts.Length > 0)
        {
            ContactPoint contact = other.contacts[0];
            GameObject hitImpactFx = Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
            Destroy(hitImpactFx, 1f);
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.Instance.ReturnBullet(gameObject);
    }
}