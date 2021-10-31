using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls the world reference of an object. Will try to add itself to the player's inventory.
/// </summary>
public class DroppedObject : MonoBehaviour
{
    private Item item;
    private int amount;
    private float retryAddTimer = 0f;
    private Transform label;
    private Transform player;
    private Inventory inventory;

    public void Construct(Item item, int amount, Color labelColor)
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        inventory = FindObjectOfType<Inventory>();

        Transform labelPrefab = Resources.Load<Transform>("Prefabs/UI/Item Label");
        Transform worldCanvas = GameObject.Find("Item Label Canvas").transform;

        this.item = item;
        this.amount = amount;

        label = Instantiate(labelPrefab, worldCanvas);
        label.GetComponent<TextMeshProUGUI>().text = "x" + amount + " " + item.itemName;
        label.GetComponent<TextMeshProUGUI>().color = labelColor;
        label.position = (Vector2)transform.position + new Vector2(0f, 1f);
    }

    private void Update()
    {
        label.transform.position = (Vector2)transform.position + new Vector2(0f, 1f);

        float distance = Vector2.Distance(player.position, transform.position);

        if (distance <= 3f && inventory.CanBeAdded(item, amount)) { 
            if (distance >= 0.5)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, Time.deltaTime * 6f);
                return;
            }

            if (retryAddTimer == 0f)
            {
                retryAddTimer -= Time.deltaTime;
                return;
            }

            if (inventory.TryAddItem(item, amount))
            {
                Destroy(label.gameObject);
                Destroy(this.gameObject);
            }

            retryAddTimer = 3f;
        }
    }
}
