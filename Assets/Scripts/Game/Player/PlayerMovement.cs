using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls basic inputs and movement calls for the player.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float movementVelocity;
    public float runningVelocityMultiplier;
    public float staminaRechargeVelocity;
    public float staminaDrainVelocity;
    public float staminaRechargeTimer;
    public Vector2 inputVelocity;
    [Header("References")]
    public Rigidbody2D playerRB;
    public Animator playerAnimator;
    public PlayerStatus playerStatus;

    private bool staminaRecharging = false;

    private void Update()
    {
        // Basic movement input.
        inputVelocity = new Vector2(Input.GetAxis("Horizontal") * movementVelocity, Input.GetAxis("Vertical") * movementVelocity);

        // Running input.
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (playerStatus.currentStamina == 0 || staminaRecharging) return;

            inputVelocity = new Vector2(inputVelocity.x * runningVelocityMultiplier, inputVelocity.y * runningVelocityMultiplier);
            playerStatus.UpdateStamina(-(Time.deltaTime * staminaDrainVelocity));
        }
        else if (!staminaRecharging)
        {
            playerStatus.UpdateStamina(Time.deltaTime * staminaRechargeTimer);
        }

        // Stamina recharge.
        if (!staminaRecharging && playerStatus.currentStamina == 0) StartCoroutine("RechargeStamina");


        playerRB.velocity = Vector2.Lerp(playerRB.velocity, inputVelocity, Time.deltaTime * movementVelocity);

        playerAnimator.SetBool("Walking", playerRB.velocity != Vector2.zero);
        playerAnimator.SetBool("Running", (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)));

    }

    private IEnumerator RechargeStamina()
    {
        playerStatus.UpdateStaminaCharging(true);
        staminaRecharging = true;

        yield return new WaitForSecondsRealtime(staminaRechargeTimer);

        while (playerStatus.currentStamina != playerStatus.maxStamina)
        {
            playerStatus.UpdateStamina(Time.deltaTime * staminaRechargeVelocity);
            yield return null;
        }

        playerStatus.UpdateStaminaCharging(false);
        staminaRecharging = false;
    }

    private void FixedUpdate()
    {
        // Player position clamping.
        // TODO: make this better...
        if (transform.position.x < 20) { transform.position = new Vector2(20f, transform.position.y); }
        if (transform.position.x > WorldData.worldSize - 20) { transform.position = new Vector2(WorldData.worldSize - 20, transform.position.y); }
        if (transform.position.y < 20) { transform.position = new Vector2(transform.position.x, 20f); }
        if (transform.position.y > WorldData.worldSize - 20) { transform.position = new Vector2(transform.position.x, WorldData.worldSize - 20); }
    }
}
