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
    public List<LayerSetterChild> updates;

    private Vector2 lastPositionCheck;

    private void Start()
    {
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
        foreach (LayerSetterChild item in updates)
        {
            int order = (int)-((transform.position.y) * 100f);

            if (item.spriteChildren)
            {
                item.spriteChildren.sortingOrder = order;
                item.spriteChildren.transform.position = new Vector3(item.spriteChildren.transform.position.x, item.spriteChildren.transform.position.y, -item.offset);
            }
            else 
            { 
                item.particleChildren.sortingOrder = order;
                item.particleChildren.transform.position = new Vector3(item.particleChildren.transform.position.x, item.particleChildren.transform.position.y, -item.offset);
            }

            lastPositionCheck = transform.position;
        }
    }
}
