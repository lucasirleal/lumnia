using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script will control simple melee attacks.
/// </summary>
public class MeleeAttaclAI : MonoBehaviour
{
    [Header("Settings")]
    public float attackResetTimer;
    public float attackRange;
    public float attackKick;
    [Header("References")]
    public Transform player;
    public UnityEvent attackFinishedCallback;
    public EnemyStatus statusHandler;

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
        if (Vector2.Distance(player.position, this.transform.position) <= attackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            player.GetComponent<Rigidbody2D>().AddForce(direction * attackKick, ForceMode2D.Impulse);
            player.GetComponent<PlayerStatus>().UpdateHealth(-statusHandler.damage);
        }

        yield return new WaitForSecondsRealtime(attackResetTimer);
        attackFinishedCallback.Invoke();
    }
}
