using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data representation of an active buff.
/// </summary>
public class ActiveBuffs
{
    public Buff buff;
    public float timer;
    public Coroutine coroutine;
    public Transform spawnedParticles;

    public ActiveBuffs(Buff buff, float timer)
    {
        this.buff = buff;
        this.timer = timer;
    }
}
