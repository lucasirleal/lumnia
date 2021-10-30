using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLayerSetter : MonoBehaviour
{
    [Header("Settings")]
    public bool autoDestroy = true;

    private ParticleSystemRenderer spriteRenderer;
    private Vector2 lastPositionCheck;

    private void Start()
    {
        spriteRenderer = GetComponent<ParticleSystemRenderer>();
        GetComponent<ParticleSystem>().Play();

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
        spriteRenderer.sortingOrder = (int)-(transform.position.y * 100f);
        lastPositionCheck = transform.position;
    }
}
