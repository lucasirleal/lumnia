using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlls the little chests that spawn on the world.
/// </summary>
public class LootBox : MonoBehaviour
{
    [Header("Data")]
    public bool isOpen;
    [Header("Drops")]
    public LootTable drops;
    [Header("References")]
    public Transform hintPrefab;
    public Transform hintCanvas;
    public Sprite openedSprite;

    private Transform currentHint;
    private bool touchingPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentHint || isOpen) return;

        if (collision.GetComponent<PlayerMovement>())
        {
            currentHint = Instantiate(hintPrefab, hintCanvas);
            currentHint.position = transform.position + new Vector3(0f, 1.2f, 0f);
            touchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!currentHint) return;

        if (collision.GetComponent<PlayerMovement>())
        {
            touchingPlayer = false;
            Destroy(currentHint.gameObject);
        }
    }

    private void Update()
    {
        if (!touchingPlayer) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Drop();
        }
    }

    /// <summary>
    /// Drops its contents.
    /// </summary>
    private void Drop()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        if (drops.useMinMax)
        {
            HandleMinMax(inventory);
        }
        else
        {
            HandleNormalDrops(inventory);
        }

        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = openedSprite;
        isOpen = true;

        if (!currentHint) return;

        Destroy(currentHint.gameObject);
    }

    /// <summary>
    /// Handle the drops using the minmax system.
    /// </summary>
    /// <param name="inventory">The inventory system.</param>
    private void HandleMinMax(Inventory inventory)
    {
        int droppedCounter = 0;

        foreach (LootTableItem item in drops.drops)
        {
            if (droppedCounter == drops.maxItems) return;

            float chance = Random.Range(0f, 100f);

            if (chance > item.chance) continue;

            if (droppedCounter + item.amount <= drops.maxItems)
            {
                inventory.DropItem(item.item, item.amount, transform.position);
                droppedCounter += item.amount;
            }
            else
            {
                inventory.DropItem(item.item, drops.maxItems - droppedCounter, transform.position);
                droppedCounter = drops.maxItems;
            }
        }

        if (droppedCounter == 0)
        {
            for (int i = 0; i < drops.minItems; i++)
            {
                Item item = drops.drops[Random.Range(0, drops.drops.Count)].item;
                inventory.DropItem(item, 1, transform.position);
            }
        }
    }

    /// <summary>
    /// Handle drops without the minmax system.
    /// </summary>
    /// <param name="inventory">The inventory system.</param>
    private void HandleNormalDrops(Inventory inventory)
    {
        foreach (LootTableItem item in drops.drops)
        {
            float chance = Random.Range(0f, 100f);

            if (chance > item.chance) continue;

            inventory.DropItem(item.item, item.amount, transform.position);
        }
    }
}
