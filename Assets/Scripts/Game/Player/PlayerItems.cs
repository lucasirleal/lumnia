using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script controls player owned items and equipments.
/// </summary>
public class PlayerItems : MonoBehaviour
{
    [Header("Items")]
    public Item mainHand;
    [Header("References")]
    public Image mainHandDisplay;
    public SpriteRenderer mainHandBodyDisplay;
    public Texture2D unequipIcon;
    public Inventory inventory;

    private void Start()
    {
        UpdateDisplays();
    }

    /// <summary>
    /// Called whenever an equipment slot is hovered.
    /// </summary>
    /// <param name="slot">The slot's icon renderer.</param>
    public void RegisterHover(Image slot)
    {
        if (slot == mainHandDisplay && !mainHand) return;

        Cursor.SetCursor(unequipIcon, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Called when the mouse leaves an equipment slot.
    /// </summary>
    public void RegisterUnhover()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Called when a slot is clicked on.
    /// </summary>
    /// <param name="slot">The slot's icon renderer.</param>
    public void RegisterClick(Image slot)
    {
        if (slot == mainHandDisplay && mainHand)
        {
            if (inventory.CanBeAdded(mainHand, 1))
            {
                if (inventory.TryAddItem(mainHand, 1)) mainHand = null; RegisterUnhover(); 
            }
        }

        UpdateDisplays();
    }

    /// <summary>
    /// Tries to equip an item.
    /// </summary>
    /// <param name="item">The target item.</param>
    /// <returns>The item that was removed from the slot.</returns>
    public Item EquipItem(Item item)
    {
        switch (item.equipType)
        {
            case Item.EquipmentType.MainHand:
                Item current = mainHand;
                mainHand = item;
                UpdateDisplays();
                return current;
            default:
                return null;
        }
    }

    /// <summary>
    /// Updates the visuals for the slots.
    /// </summary>
    private void UpdateDisplays()
    {
        if (mainHand == null) 
        { 
            mainHandDisplay.color = Color.clear;
            mainHandBodyDisplay.color = Color.clear;
        }
        else 
        { 
            mainHandDisplay.sprite = mainHand.inventoryArt; 
            mainHandDisplay.color = Color.white;

            mainHandBodyDisplay.sprite = mainHand.worldArt;
            mainHandBodyDisplay.color = Color.white;
            mainHandBodyDisplay.transform.localPosition = mainHand.equipedOffset;
        }
    }
}
