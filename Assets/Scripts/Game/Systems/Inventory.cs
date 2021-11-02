using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This script controls the UI updates for the inventory and it's basic settings.
/// </summary>
public class Inventory : MonoBehaviour
{
    [Header("Data")]
    public int inventorySize;
    [Header("References")]
    public Transform uiSlotPrefab;
    public Transform slotHolder;
    public GameObject inventoryExamineHolder;
    public TextMeshProUGUI inventoryExamineTitle;
    public TextMeshProUGUI inventoryExamineDescription;
    public Image inventoryExamineIcon;
    [Header("Icon References")]
    public Texture2D equipIcon;
    [Header("Dropped items references")]
    public Transform droppedItemPrefab;
    [Header("Color References")]
    public Color commonColor;
    public Color uncommonColor;
    public Color rareColor;
    public Color legendaryColor;
    public Color epicColor;

    private List<InventorySlot> slots;
    private Dictionary<string, Item> loadedItems;
    private PlayerItems equipmentHandler;
    private ObjectPlacement placementHandler;

    private void Start()
    {
        equipmentHandler = FindObjectOfType<PlayerItems>();
        placementHandler = FindObjectOfType<ObjectPlacement>();

        LoadItems();
        slots = new List<InventorySlot>();

        ConstructIventory();
        HideExamine();
        UpdateVisuals();
    }

    /// <summary>
    /// Loads all items from the resources folder.
    /// </summary>
    private void LoadItems()
    {
        Item[] items = Resources.LoadAll<Item>("Items");
        loadedItems = new Dictionary<string, Item>();

        foreach (Item item in items)
        {
            loadedItems.Add(item.itemName.ToLower(), item);
        }
    }

    /// <summary>
    /// Drops an item based on its name.
    /// </summary>
    /// <param name="itemName">The item's name.</param>
    /// <param name="position">The position it should be dropped.</param>
    public void DropItem(string itemName, int amount, Vector2 position)
    {
        Item item = loadedItems[itemName.ToLower()];
        DropItem(item, amount, position);
    }

    /// <summary>
    /// Drops a loot table.
    /// </summary>
    /// <param name="table">The table to be dropped.</param>
    /// <param name="position">The position it should be dropped.</param>
    public void DropLootTable(LootTable table, Vector2 position)
    {
        if (table.useMinMax)
        {
            HandleMinMax(table, position);
        }
        else
        {
            HandleNormalDrops(table, position);
        }
    }

    /// <summary>
    /// Handle the drops using the minmax system.
    /// </summary>
    /// <param name="table">The loot table to be dropped.</param>
    /// <param name="position">The position it should be dropped.</param>
    private void HandleMinMax(LootTable table, Vector2 position)
    {
        int droppedCounter = Random.Range(table.minRolls, table.maxRolls + 1);

        for (int i = 0; i < droppedCounter; i++)
        {
            // Stores the biggest drop rate, this will be the default
            // if no other objects are dropped.
            LootTableItem defaultItem = new LootTableItem();
            defaultItem.chance = 0f;

            bool spawnDefault = true;

            foreach (LootTableItem item in table.drops)
            {
                float chance = Random.Range(0f, 100f);

                if (item.chance > defaultItem.chance) defaultItem = item;
                if (chance > item.chance) continue;

                if (item.useFixedAmount) DropItem(item.item, item.fixedAmount, position);
                else DropItem(item.item, Random.Range(item.minAmount, item.maxAmount + 1), position);
                spawnDefault = false;
            }

            if (spawnDefault)
            {
                if (defaultItem.useFixedAmount) DropItem(defaultItem.item, defaultItem.fixedAmount, position);
                else DropItem(defaultItem.item, Random.Range(defaultItem.minAmount, defaultItem.maxAmount + 1), position);
            }
        }
    }

    /// <summary>
    /// Handle drops without the minmax system.
    /// </summary>
    /// <param name="table">The loot table to be dropped.</param>
    /// <param name="position">The position it should be dropped.</param>
    private void HandleNormalDrops(LootTable table, Vector2 position)
    {
        foreach (LootTableItem item in table.drops)
        {
            float chance = Random.Range(0f, 100f);

            if (chance > item.chance) continue;

            if (item.useFixedAmount) DropItem(item.item, item.fixedAmount, position);
            else DropItem(item.item, Random.Range(item.minAmount, item.maxAmount + 1), position);
        }
    }

    /// <summary>
    /// Drops an item based on its reference.
    /// </summary>
    /// <param name="item">The item</param>
    /// <param name="position">The position it should be dropped.</param>
    public void DropItem(Item item, int amount, Vector2 position)
    {
        Transform clone = Instantiate(droppedItemPrefab, position, Quaternion.identity);

        switch (item.rarity)
        {
            case Item.ItemRarity.Common:
                clone.GetComponent<DroppedObject>().Construct(item, amount, commonColor);
                break;
            case Item.ItemRarity.Uncommon:
                clone.GetComponent<DroppedObject>().Construct(item, amount, uncommonColor);
                break;
            case Item.ItemRarity.Rare:
                clone.GetComponent<DroppedObject>().Construct(item, amount, rareColor);
                break;
            case Item.ItemRarity.Legendary:
                clone.GetComponent<DroppedObject>().Construct(item, amount, legendaryColor);
                break;
            default:
                clone.GetComponent<DroppedObject>().Construct(item, amount, epicColor);
                break;
        }
    }

    /// <summary>
    /// Constructs the initial list for inventory slots.
    /// Also sets their mouse events on runtime.
    /// </summary>
    private void ConstructIventory()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            Transform clone = Instantiate(uiSlotPrefab, slotHolder);

            EventTrigger trigger = clone.GetComponent<EventTrigger>();

            EventTrigger.Entry enter = new EventTrigger.Entry();
            enter.eventID = EventTriggerType.PointerEnter;
            enter.callback.AddListener((eventData) => { ExamineItem(clone.gameObject); });

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((eventData) => { HideExamine(); });

            EventTrigger.Entry click = new EventTrigger.Entry();
            click.eventID = EventTriggerType.PointerDown;
            click.callback.AddListener((eventData) => { RegisterClick(clone.gameObject); });

            trigger.triggers.Add(enter);
            trigger.triggers.Add(exit);
            trigger.triggers.Add(click);

            slots.Add(new InventorySlot(clone.gameObject));
        }
    }

    /// <summary>
    /// Called whenever a player clicks on an item.
    /// </summary>
    /// <param name="prefab">The inventory slot UI hook.</param>
    public void RegisterClick(GameObject prefab)
    {
        int index = slots.IndexOf(slots.Find((x) => x.uiHook == prefab));

        if (index < 0 || index >= slots.Count) return;

        Item item = slots[index].item;

        if (item == null) return;

        switch (item.click)
        {
            case Item.LeftClickDefault.Equip:
                Item returnItem = equipmentHandler.EquipItem(item);
                ForceRemoveItem(index);
                TryAddItem(returnItem, 1);
                HideExamine();
                ExamineItem(prefab);
                break;
            case Item.LeftClickDefault.Place:
                placementHandler.TriggerPlace(item);
                break;
            default:
                return;
        }
    }

    /// <summary>
    /// Called whenever the player hovers over an item in the inventory.
    /// </summary>
    /// <param name="prefab">The inventory slot UI hook.</param>
    public void ExamineItem(GameObject prefab)
    {
        int index = slots.IndexOf(slots.Find((x) => x.uiHook == prefab));

        if (index < 0 || index >= slots.Count) return;

        Item item = slots[index].item;

        if (item == null) return;

        inventoryExamineIcon.sprite = item.inventoryArt;
        inventoryExamineTitle.text = item.itemName;
        inventoryExamineDescription.text = item.description;

        inventoryExamineHolder.SetActive(true);

        switch (item.click)
        {
            case Item.LeftClickDefault.Equip:
                Cursor.SetCursor(equipIcon, Vector2.zero, CursorMode.Auto);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Hides the examine screen.
    /// </summary>
    public void HideExamine()
    {
        inventoryExamineHolder.SetActive(false);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Checks if an item can be added to the backpack.
    /// </summary>
    /// <param name="item">The target item.</param>
    /// <param name="amount">It's amount.</param>
    /// <returns>True if it can be added.</returns>
    public bool CanBeAdded(Item item, int amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null) return true;
            if (slot.item == item && slot.amount + amount <= item.maxStack) return true;
        }

        return false;
    }

    /// <summary>
    /// Try to add a new item to the backpack.
    /// </summary>
    /// <param name="item">The target item.</param>
    /// <param name="amount">It's amount.</param>
    /// <returns>True if it was added.</returns>
    public bool TryAddItem(Item item, int amount)
    {
        if (item == null) return false;

        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item && slot.amount + amount <= item.maxStack)
            {
                slot.amount += amount;
                UpdateVisuals();
                return true;
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.amount = amount;
                UpdateVisuals();
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Updates the UI for the inventory.
    /// </summary>
    private void UpdateVisuals()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                slot.uiHook.transform.GetChild(0).GetComponent<Image>().color = Color.clear;
                slot.uiHook.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                continue;
            }

            slot.uiHook.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.inventoryArt;
            slot.uiHook.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            slot.uiHook.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + slot.amount;
        }
    }

    /// <summary>
    /// Forcibly removes an item from the inventory.
    /// </summary>
    /// <param name="slot">The slot index.</param>
    private void ForceRemoveItem(int slot)
    {
        if (slot < 0 || slot >= slots.Count) return;

        slots[slot].item = null;

        UpdateVisuals();
    }

    /// <summary>
    /// Tries to remove an item.
    /// </summary>
    /// <param name="target">The item.</param>
    /// <param name="amount">The amount to be removed.</param>
    /// <returns>True if all the items were removed.</returns>
    public bool RemoveItem(Item target, int amount)
    {
        // Validation first.
        int amountValidated = amount;
        foreach (InventorySlot item in slots)
        {
            if (item.item == target) amountValidated -= item.amount;

            if (amountValidated <= 0) break;
        }

        if (amountValidated > 0) return false;

        int amountToRemove = amount;
        foreach (InventorySlot item in slots.ToArray())
        {
            if (item.item == target)
            {
                if (item.amount >= amountToRemove)
                {
                    item.amount -= amountToRemove;
                    amountToRemove = 0;
                }
                else
                {
                    amountToRemove -= item.amount;
                    item.amount = 0;
                }

                if (item.amount == 0) ForceRemoveItem(slots.IndexOf(item));

                if (amountToRemove == 0) break;
            }
        }

        UpdateVisuals();
        return true;
    }
}
