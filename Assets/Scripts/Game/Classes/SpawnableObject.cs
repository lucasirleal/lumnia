using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information about an object that can be spawned in the world.
/// </summary>
[System.Serializable]
public class SpawnableObject
{
    public Transform prefab;
    public Transform parentHolder;
    [Range(0f, 100f)]
    public float chanceToSpawn;
    public bool destroyableOnPlaceOver;

    /// <summary>
    /// Should this object be spawned?
    /// </summary>
    /// <returns>True if a random number happend to fall under the spawning chance.</returns>
    public bool ShouldSpawn()
    {
        return Random.Range(0f, 100f) < chanceToSpawn;
    }
}
