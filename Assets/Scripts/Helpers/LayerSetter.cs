using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the layer of the sprite renderers to match the negative Y value of its world position.
/// </summary>
public class LayerSetter : MonoBehaviour
{
    [Header("Settings")]
    public bool autoDestroy = true;
    public int offset;

    private SpriteRenderer spriteRenderer;
    private Vector2 lastPositionCheck;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        CalculateOrder();
        if (autoDestroy) { Destroy(this); }
    }

    private void Update()
    {
        if ((Vector2)transform.position == lastPositionCheck) { return; }

        CalculateOrder();
    }

    /// <summary>
    /// Sets the sorting order.
    /// </summary>
    private void CalculateOrder()
    {
        spriteRenderer.sortingOrder = (int)-((transform.position.y * 100f) + offset);
        lastPositionCheck = transform.position;
    }
}
