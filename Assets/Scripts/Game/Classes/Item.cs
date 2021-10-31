using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The data representation of an item.
/// </summary>
[CreateAssetMenu(fileName = "Item")]
public class Item : ScriptableObject
{
    [Header("Basic settings")]
    public string itemName;
    [TextArea]
    public string description;
    public int maxStack = 1;
    public ItemRarity rarity;
    [Header("Artworks")]
    public Sprite worldArt;
    public Sprite inventoryArt;
    [Header("Equipable settings")]
    public Vector2 equipedOffset;

    public enum ItemRarity
    {
        Common, Uncommon, Rare, Legendary, Epic
    }
}
