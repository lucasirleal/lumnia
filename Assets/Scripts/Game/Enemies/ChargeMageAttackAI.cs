using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A simple AI component that will charge an attack an then create
/// an instanced prefab with the desired configurations.
/// </summary>
public class ChargeMageAttackAI : MonoBehaviour
{
    [Header("Settings")]
    public float chargingTime;
    public float attackResetTimer;
    public float maxPrecisionDeviance;
    public Vector2 projectileSpawnOffset;
    public ProjectileType type;
    [Header("References")]
    public Transform player;
    public GameObject chargingDisplay;
    public Transform projectilePrefab;
    public UnityEvent attackFinishedCallback;
    public EnemyStatus statusHandler;

    public enum ProjectileType
    {
        SimpleFireBall
    }
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    /// <summary>
    /// Starts the cicle for an attack.
    /// </summary>
    public void TriggerAttack()
    {
        StartCoroutine("AttackCicle");
    }

    private IEnumerator AttackCicle()
    {
        chargingDisplay.SetActive(true);
        yield return new WaitForSecondsRealtime(chargingTime);
        chargingDisplay.SetActive(false);

        Transform projectile = Instantiate(projectilePrefab, (Vector2)transform.position + projectileSpawnOffset, Quaternion.identity);
        SetUpProjectile(projectile);

        yield return new WaitForSecondsRealtime(attackResetTimer);
        attackFinishedCallback.Invoke();
    }

    /// <summary>
    /// Calculates the target position of a projectile based on the precision deviancy.
    /// </summary>
    /// <returns>A vector2 with the position.</returns>
    private Vector2 GetTarget()
    {
        return new Vector2(player.position.x + Random.Range(-maxPrecisionDeviance, maxPrecisionDeviance), player.position.y + Random.Range(-maxPrecisionDeviance, maxPrecisionDeviance));
    }

    /// <summary>
    /// Calls the constructor of a projectile based on the enemy type.
    /// </summary>
    /// <param name="clone">The projectile.</param>
    private void SetUpProjectile(Transform clone)
    {
        switch (type)
        {
            case ProjectileType.SimpleFireBall:
                clone.GetComponent<SimpleProjectile>().Construct(GetTarget(), true, statusHandler.damage);
                break;
            default:
                break;
        }
    }
}
