using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A group of items with chances.
/// </summary>
[System.Serializable]
public class LootTableItem
{
    [Range(0f, 100f)]
    public float chance;
    public Item item;
    public bool useFixedAmount;
    public int fixedAmount;
    public int minAmount;
    public int maxAmount;
}
