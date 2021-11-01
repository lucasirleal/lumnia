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
    private float damage;
    private EnemySpawner enemies;

    /// <summary>
    /// Constructs the projectile.
    /// </summary>
    /// <param name="target">Where should it land.</param>
    /// <param name="isEnemyProjectile">Was it created by an enemy? (If true, only damages the player.)</param>
    public void Construct(Vector2 target, bool isEnemyProjectile, float damage)
    {
        this.target = target;
        this.isEnemy = isEnemyProjectile;
        this.damage = damage;
        enemies = FindObjectOfType<EnemySpawner>();
        StartCoroutine("Action");
    }

    private IEnumerator Action()
    {
        while (!forceReached && Vector2.Distance(transform.position, target) >= 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * projectileSpeed);

            if (!isEnemy && shouldStopMidway)
            {
                foreach (Transform item in enemies.spawnedEnemies.ToArray())
                {
                    if (item == null || item.gameObject == null) continue;
                    if (Vector2.Distance(transform.position, item.position) <= 0.5f) forceReached = true;
                    break;
                }
            }

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
        else CalculateExplosionForPlayer();

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

    /// <summary>
    /// Creates a force that pushes all enemies based on its distance.
    /// </summary>
    private void CalculateExplosionForPlayer()
    {
        foreach (Transform item in enemies.spawnedEnemies.ToArray())
        {
            if (item == null || item.gameObject == null || (!item.GetComponent<Rigidbody2D>() || !item.GetComponent<EnemyStatus>())) continue;
            if (Vector2.Distance(transform.position, item.position) > areaOfEffect) continue;

            Vector2 direction = (item.position - transform.position).normalized;
            item.GetComponent<Rigidbody2D>().AddForce(direction * explosionKick, ForceMode2D.Impulse);
            item.GetComponent<EnemyStatus>().UpdateHealth(-damage);
        }
    }
}
