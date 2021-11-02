using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGenerator.UnityPort;
using UnityEngine.Tilemaps;

/// <summary>
/// Creates a hook between the main game and the generator tool.
/// </summary>
public class WorldGenerator : MonoBehaviour
{
    [Header("Settings")]
    public int worldSize;
    [Header("Object references")]
    public SpawnableObject[] waterBiomeObjects;
    public SpawnableObject[] forestBiomeObjects;
    [Header("Tile References")]
    public TileBase deepWaterTile;
    public TileBase shallowWaterTile;
    public TileBase forestTile;
    [Header("Grid References")]
    public Tilemap definitiveGroundGrid;
    public Tilemap definitiveWaterGrid;
    [Header("References")]
    public Sprite deepWaterMarker;
    public Sprite shallowWaterMarker;
    public Sprite forestMarker;

    private MapGeneratorTool generator;

    private Tilemap placeholderWaterGrid;
    private Tilemap placeholderGroundGrid;

    private Vector2 validSpawnPointFound;
    private float validSpawnPointDistance = Mathf.Infinity;

    void Start()
    {
        WorldData.Construct(worldSize);
        InitialGeneratorSetUp();
        GeneratePlaceholderMap();
        ConvertPlaceholders();
        ClearPlaceholders();
        SetDefinitiveTiles();
        SpawnObjects();
        MovePlayerToSpawn();
    }

    /// <summary>
    /// Grabs the reference for the generator and sets its sizes.
    /// </summary>
    private void InitialGeneratorSetUp()
    {
        generator = GetComponent<MapGeneratorTool>();

        generator.width = worldSize;
        generator.height = worldSize;
    }

    /// <summary>
    /// Generates a placeholder map and stores its reference.
    /// </summary>
    private void GeneratePlaceholderMap()
    {
        generator.TryGenerate();

        Transform placeholderParent = this.transform.GetChild(0);
        placeholderGroundGrid = placeholderParent.GetChild(0).GetComponent<Tilemap>();
        placeholderWaterGrid = placeholderParent.GetChild(1).GetComponent<Tilemap>();
    }

    /// <summary>
    /// Converts all placeholder tiles into the actual world tiles.
    /// </summary>
    private void ConvertPlaceholders()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                WorldTile tile = ConvertTile(position);

                if (tile != null)
                {
                    WorldData.StoreTile(tile, (Vector2Int)position);
                    // Defines the spawning point of the world.
                    if (!tile.isWater())
                    {
                        if (Vector2.Distance(new Vector2(x, y), new Vector2(worldSize / 2f, worldSize / 2f)) < validSpawnPointDistance)
                        {
                            validSpawnPointFound = new Vector2(x, y);
                            validSpawnPointDistance = Vector2.Distance(new Vector2(x, y), new Vector2(worldSize / 2f, worldSize / 2f));
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Converts a placeholder tile into a world tile and returns it.
    /// </summary>
    /// <param name="position">The placeholder tile's position.</param>
    /// <returns>The converted tile.</returns>
    private WorldTile ConvertTile(Vector3Int position)
    {
        Sprite waterMarker = placeholderWaterGrid.GetSprite(position);
        Sprite groundMarker = placeholderGroundGrid.GetSprite(position);

        if (waterMarker)
        {
            if (waterMarker == shallowWaterMarker) return new WorldTile(WorldTile.Biome.ShallowWater, (Vector2Int)position);
            if (waterMarker == deepWaterMarker) return new WorldTile(WorldTile.Biome.DeepWater, (Vector2Int)position);
        }

        if (groundMarker)
        {
            if (groundMarker == forestMarker) return new WorldTile(WorldTile.Biome.Forest, (Vector2Int)position);
        }

        return null;
    }

    /// <summary>
    /// Deletes the placeholder tiles.
    /// </summary>
    private void ClearPlaceholders()
    {
        Destroy(this.transform.GetChild(0).gameObject);
        Destroy(this.transform.GetChild(1).gameObject);
    }

    /// <summary>
    /// Places the definitive tiles based on the placeholder's position.
    /// </summary>
    private void SetDefinitiveTiles()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                switch (WorldData.worldTiles[x, y].biome)
                {
                    case WorldTile.Biome.DeepWater:
                        definitiveWaterGrid.SetTile(position, deepWaterTile);
                        break;
                    case WorldTile.Biome.ShallowWater:
                        definitiveWaterGrid.SetTile(position, shallowWaterTile);
                        break;
                    case WorldTile.Biome.Forest:
                        definitiveGroundGrid.SetTile(position, forestTile);
                        break;
                    default:
                        Debug.LogError("Unkown tile type: " + WorldData.worldTiles[x, y].biome);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Moves the player to the valid central point of the world.
    /// </summary>
    private void MovePlayerToSpawn()
    {
        FindObjectOfType<PlayerMovement>().transform.position = validSpawnPointFound;
    }

    /// <summary>
    /// Spawns all world objects.
    /// </summary>
    private void SpawnObjects()
    {
        Vector2 offset = new Vector2(0.5f, 0.5f);

        foreach (WorldTile item in WorldData.worldTiles)
        {
            if (item.isWater())
            {
                SpawnableObject targetObject = GetObjectToSpawn(waterBiomeObjects);
                if (targetObject != null) { Instantiate(targetObject.prefab, (Vector2)item.position + offset, Quaternion.identity, targetObject.parentHolder); }
            }
            else if (item.biome == WorldTile.Biome.Forest)
            {
                SpawnableObject targetObject = GetObjectToSpawn(forestBiomeObjects);
                if (targetObject == null) continue;

                Transform clone = Instantiate(targetObject.prefab, (Vector2)item.position + offset, Quaternion.identity, targetObject.parentHolder);
                WorldData.StoreObject(new WorldObject(clone.gameObject, targetObject.destroyableOnPlaceOver), item.position);
            }
        }
    }

    /// <summary>
    /// Checks if any object should spawn.
    /// </summary>
    /// <param name="collection">The collection of objects.</param>
    private SpawnableObject GetObjectToSpawn(SpawnableObject[] collection)
    {
        foreach (SpawnableObject item in collection)
        {
            if (item.ShouldSpawn()) return item;
        }

        return null;
    }
}
