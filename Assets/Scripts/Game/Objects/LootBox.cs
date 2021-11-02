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
            isOpen = true;
            this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = openedSprite;
            FindObjectOfType<Inventory>().DropLootTable(drops, transform.position);

            if (!currentHint) return;

            Destroy(currentHint.gameObject);
        }
    }


}
