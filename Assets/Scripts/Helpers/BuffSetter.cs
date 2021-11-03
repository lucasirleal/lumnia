using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script works as an editor hook for applying a buff at certain times.
/// Will probably be called by a UnityEvent function.
/// On it's own, it doesn't do much. 
/// </summary>
public class BuffSetter : MonoBehaviour
{
    [Header("Hook settings")]
    public Buff buff;

    /// <summary>
    /// Applies the buff to the player.
    /// </summary>
    public void ApplyToPlayer()
    {
        FindObjectOfType<PlayerStatus>().ApplyBuff(buff);
    }
}
