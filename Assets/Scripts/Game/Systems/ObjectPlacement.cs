using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Controlls the actions of placing an object in the world.
/// </summary>
public class ObjectPlacement : MonoBehaviour
{
    [Header("Settings")]
    public Color successColor;
    public Color errorColor;
    [Header("Data")]
    public Item currentObjectBeingPlaced;

    private Transform draggablePrefab;
    private Inventory inventory;

    private Vector2 mousePos;
    private Vector2Int idPos;
    private Vector2 clampedPos;
    private WorldObject objectBelow;
    private bool canPlace;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    /// <summary>
    /// Triggers the action of placing an object.
    /// </summary>
    /// <param name="target">The target object.</param>
    public void TriggerPlace(Item target)
    {
        if (target.click != Item.LeftClickDefault.Place) return;

        currentObjectBeingPlaced = target;

        Cursor.visible = false;

        if (draggablePrefab) Destroy(draggablePrefab.gameObject);

        draggablePrefab = Instantiate(target.draggableObject);
    }

    /// <summary>
    /// Triggered when the player clicks while placing an object.
    /// </summary>
    private void CompletePlace()
    {
        if (!inventory.RemoveItem(currentObjectBeingPlaced, 1))
        {
            CancelPlacement();
            return;
        }

        Instantiate(currentObjectBeingPlaced.worldObject, clampedPos, Quaternion.identity);
        DestroyObjectBelow();
        CancelPlacement();
    }

    /// <summary>
    /// Cancels all actions of placement.
    /// </summary>
    public void CancelPlacement()
    {
        if (draggablePrefab) Destroy(draggablePrefab.gameObject);

        Cursor.visible = true;
        currentObjectBeingPlaced = null;
    }

    /// <summary>
    /// Destroyies the object at the mouse position.
    /// </summary>
    private void DestroyObjectBelow()
    {
        if (objectBelow != null && objectBelow.objectAttached != null)
        {
            Destroy(objectBelow.objectAttached);
        }
    }

    private void Update()
    {
        if (draggablePrefab)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            idPos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
            clampedPos = new Vector2(idPos.x + 0.5f, idPos.y + 0.5f);

            if ((Vector2)draggablePrefab.position != clampedPos) draggablePrefab.position = Vector2.Lerp(draggablePrefab.position, clampedPos, Time.deltaTime * 7f);

            objectBelow = WorldData.worldObjects[idPos.x, idPos.y];

            if (objectBelow == null || objectBelow.destroyableOnPlaceOver)
            {
                draggablePrefab.GetComponent<SpriteRenderer>().color = successColor;
                canPlace = true;
            }
            else
            {
                draggablePrefab.GetComponent<SpriteRenderer>().color = errorColor;
                canPlace = false;
            }

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && canPlace) CompletePlace();
        }
    }
}
