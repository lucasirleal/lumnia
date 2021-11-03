using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A data representation of a buff/debuff.
/// </summary>
[CreateAssetMenu(fileName = "Buff or Debuff")]
public class Buff: ScriptableObject
{
    [Header("Settings")]
    public string buffName;
    public Type type;
    public bool useFixedTime;
    public float fixedTime;
    public float minTimer;
    public float maxTimer;
    [Header("References")]
    public Transform specialParticles;

    public enum Type
    {
        Slowness, Poison
    }
}
