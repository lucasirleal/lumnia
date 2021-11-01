using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles single clicks on the map and skill interactions.
/// </summary>
public class PlayerAttacks : MonoBehaviour
{
    [Header("References")]
    public PlayerItems equipmentHandler;
    [Header("Particle References")]
    public Transform fireballProjectile;

    private float recurringTimer = 0f;

    private void Update()
    {
        if (recurringTimer > 0f) recurringTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleClick();
        }
    }

    /// <summary>
    /// Validates and handles a single auto attack order.
    /// </summary>
    private void HandleClick()
    {
        if (recurringTimer > 0f) return;

        Item mainHand = equipmentHandler.mainHand;

        if (mainHand == null) return;

        switch (mainHand.attackType)
        {
            case Item.MainAttackType.Fireball:
                ShootFireball(mainHand);
                break;
            default:
                return;
        }
    }

    /// <summary>
    /// Shots a single fireball.
    /// </summary>
    private void ShootFireball(Item mainHand)
    {
        recurringTimer = mainHand.mainAttackDelay;

        Transform clone = Instantiate(fireballProjectile, transform.position, Quaternion.identity);

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clone.GetComponent<SimpleProjectile>().Construct(pos, false, mainHand.damage);
    }
}
