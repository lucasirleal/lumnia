using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Controlls the mood and scenery for the main menu entrance and background.
/// </summary>
public class MenuScenery : MonoBehaviour
{
    [Header("Settings")]
    [Range(0f, 100f)]
    public float propSpawnChance;
    [Header("Appearence")]
    public Tile[] grassTiles;
    public Transform[] props;
    [Header("References")]
    public Tilemap backgroundTilemap;

    void Start()
    {
        ScreenFade.FadeIn();
        GenerateScenery();
        ScreenFade.FadeOut();
    }

    /// <summary>
    /// Generates a little background for the main menu!
    /// </summary>
    private void GenerateScenery()
    {
        Transform propHolder = GetPropHolder();

        Vector2 minPos = CameraUtils.ActiveCameraMinBounds();
        Vector2 maxPos = CameraUtils.ActiveCameraMaxBounds();

        for (int x = (int)minPos.x; x < maxPos.x; x++)
        {
            for (int y = (int)minPos.y; y < maxPos.y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);

                if (backgroundTilemap.GetTile(tilePos) != null) { continue; }

                SetProps(tilePos, propHolder);
                SetTile(tilePos);
            }
        }
    }

    /// <summary>
    /// Generates a prop holder.
    /// </summary>
    /// <returns>The holder's transform.</returns>
    private Transform GetPropHolder()
    {
        GameObject holder = new GameObject("Prop Holder");
        holder.transform.SetParent(this.transform);
        return holder.transform;
    }

    /// <summary>
    /// Places grass tiles.
    /// </summary>
    private void SetTile(Vector3Int position)
    {
        int tileIndex = Random.Range(0, grassTiles.Length);
        backgroundTilemap.SetTile(position, grassTiles[tileIndex]);
    }

    /// <summary>
    /// Places props on a tile.
    /// </summary>
    private void SetProps(Vector3Int position, Transform holder)
    {
        float chance = Random.Range(0f, 100f);
        if (chance > propSpawnChance) { return; }

        int propIndex = Random.Range(0, props.Length);
        Instantiate(props[propIndex], position, Quaternion.identity, holder);
    }
}
