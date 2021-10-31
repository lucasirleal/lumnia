using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will control a very simple projectile.
/// Go to location -> destroy itself.
/// </summary>
public class SimpleProjectile : MonoBehaviour
{
    [Header("Settings")]
    public bool shouldStopMidway;
    public float projectileSpeed;
    public float explosionKick;
    public Transform explosionParticles;
    public float areaOfEffect;

    private bool forceReached;
    private Vector2 target;
    private bool isEnemy;
    private int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!shouldStopMidway || isEnemy) return;
        forceReached = true;
    }

    /// <summary>
    /// Constructs the projectile.
    /// </summary>
    /// <param name="target">Where should it land.</param>
    /// <param name="isEnemyProjectile">Was it created by an enemy? (If true, only damages the player.)</param>
    public void Construct(Vector2 target, bool isEnemyProjectile, int damage)
    {
        this.target = target;
        this.isEnemy = isEnemyProjectile;
        this.damage = damage;
        StartCoroutine("Action");
    }

    private IEnumerator Action()
    {
        while (!forceReached && Vector2.Distance(transform.position, target) >= 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * projectileSpeed);
            yield return null;
        }

        TriggerReached();
    }

    /// <summary>
    /// Makes it so the projectile explodes.
    /// </summary>
    private void TriggerReached()
    {
        Destroy(Instantiate(explosionParticles, transform.position, Quaternion.identity).gameObject, 3f);

        if (isEnemy) CalculateExplosionForEnemy();

        Destroy(this.gameObject);
    }

    /// <summary>
    /// Creates a force that pushes the player based on its distance.
    /// </summary>
    private void CalculateExplosionForEnemy()
    {
        Transform player = FindObjectOfType<PlayerMovement>().transform;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > areaOfEffect) return;

        Vector2 direction = (player.position - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(direction * explosionKick, ForceMode2D.Impulse);
        player.GetComponent<PlayerStatus>().UpdateHealth(-damage);
    }
}
