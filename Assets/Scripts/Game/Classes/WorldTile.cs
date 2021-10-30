using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores a reference containing all important data about any given tile.
/// </summary>
public class WorldTile
{
    public Biome biome { get; private set; }
    public Vector2Int position { get; private set; }

    public enum Biome
    {
        DeepWater, ShallowWater, Forest
    }

    /// <summary>
    /// Checks wether or not a tile is ocean/lake/water.
    /// </summary>
    /// <returns>True if it is water.</returns>
    public bool isWater()
    {
        return biome == Biome.DeepWater || biome == Biome.ShallowWater;
    }

    public WorldTile(Biome biome, Vector2Int position)
    {
        this.position = position;
        this.biome = biome;
    }
}
