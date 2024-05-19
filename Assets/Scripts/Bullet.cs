using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Update()
    {
        // Return after 7 seconds if it doesn't hit an anything
        StartCoroutine(ReturnToPoolAfterDelay(7f)); 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Enemy damage logic 
            ObjectPool.Instance.ReturnBullet(gameObject);
        }
        else
            StartCoroutine(ReturnToPoolAfterDelay(3f));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.Instance.ReturnBullet(gameObject);
    }
}