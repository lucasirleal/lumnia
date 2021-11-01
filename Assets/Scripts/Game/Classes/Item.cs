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
    public LeftClickDefault click;
    [Header("Artworks")]
    public Sprite worldArt;
    public Sprite inventoryArt;
    [Header("Equipable Settings")]
    public Vector2 equipedOffset;
    public EquipmentType equipType;
    [Header("Weapon Settings")]
    public MainAttackType attackType;
    public float mainAttackDelay;
    public float damage;

    public enum ItemRarity
    {
        Common, Uncommon, Rare, Legendary, Epic
    }

    public enum MainAttackType
    {
        None, Fireball
    }

    public enum LeftClickDefault
    {
        None, Equip
    }

    public enum EquipmentType
    {
        None, MainHand
    }
}
