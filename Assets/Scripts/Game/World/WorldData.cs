using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information about the whole world.
/// </summary>
public static class WorldData
{
    public static int worldSize { get; private set; }
    public static WorldTile[,] worldTiles { get; private set; }
    public static WorldObject[,] worldObjects { get; private set; }

    /// <summary>
    /// Constructs the script.
    /// </summary>
    /// <param name="worldSize">The size of the world.</param>
    public static void Construct(int _worldSize)
    {
        worldSize = _worldSize;
        worldTiles = new WorldTile[worldSize, worldSize];
        worldObjects = new WorldObject[worldSize, worldSize];
    }

    /// <summary>
    /// Stores a tile in the world record.
    /// </summary>
    /// <param name="tile">The tile to be added.</param>
    /// <param name="position">Its position on the world.</param>
    public static void StoreTile(WorldTile tile, Vector2Int position)
    {
        worldTiles[position.x, position.y] = tile;
    }

    /// <summary>
    /// Stores an object in the world record.
    /// </summary>
    /// <param name="targetObject">The object to be added.</param>
    /// <param name="position">Its position on the world.</param>
    public static void StoreObject(WorldObject targetObject, Vector2Int position)
    {
        worldObjects[position.x, position.y] = targetObject;
    }
}
