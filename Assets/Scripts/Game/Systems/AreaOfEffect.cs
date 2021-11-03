using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An area of effect that will constantly apply a buff to the player when touched.
/// </summary>
public class AreaOfEffect : MonoBehaviour
{
    [Header("Settings")]
    public Buff buffToApply;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Player") return;
        collision.GetComponent<PlayerStatus>().ApplyBuff(buffToApply);
    }
}
