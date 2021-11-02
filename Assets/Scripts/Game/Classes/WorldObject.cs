using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps a data reference to every single object in the world.
/// </summary>
public class WorldObject
{
    public GameObject objectAttached;
    public bool destroyableOnPlaceOver; // When the player tries to put an item on the same position as this object, should we allow it?

    public WorldObject(GameObject objectAttached, bool destroyableOnPlaceOver)
    {
        this.objectAttached = objectAttached;
        this.destroyableOnPlaceOver = destroyableOnPlaceOver;
    }
}
