using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private int health = 100;

    private Transform player;
    private Animator animator;
    private bool isChasing = false;

    // Object pool reference
    private ObjectPool enemyPool;

    // Method to set object pool reference
    public void SetObjectPool(ObjectPool pool)
    {
        enemyPool = pool;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health = 100; // Reset health when object is reused from pool
    }

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= stoppingDistance)
        {
            isChasing = false;
            animator.SetBool("isRunning", false);
            return;
        }

        isChasing = true;
        animator.SetBool("isRunning", true);

        // Rotate towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Move towards the player
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("Attack");
        }
    }

    public void TakeDamage(int damage)
    {
        if (health >= 100)
        {
            health -= damage;
            animator.SetTrigger("GetHit");
        }
        else if (health <= 0)
            Die();
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(2f); // Wait for death animation to finish
        enemyPool.ReturnEnemy(gameObject); // Return this enemy object to the object pool
    }
}
