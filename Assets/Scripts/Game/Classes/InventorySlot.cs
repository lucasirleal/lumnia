using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information abount a given inventory slot.
/// </summary>
public class InventorySlot
{
    public Item item;
    public int amount;
    public GameObject uiHook;

    public InventorySlot(GameObject hook)
    {
        uiHook = hook;
    }
}
