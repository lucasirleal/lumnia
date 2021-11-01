using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script representation of randomizing a group of items.
/// </summary>
[System.Serializable]
public class LootTable
{
    [Header("Data")]
    public int minItems;
    public int maxItems;
    public bool useMinMax;
    public List<LootTableItem> drops;
}
