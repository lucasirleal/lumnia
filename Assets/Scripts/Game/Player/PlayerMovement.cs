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
    public Vector2 inputVelocity;
    [Header("References")]
    public Rigidbody2D playerRB;
    public Animator playerAnimator;
    private void Update()
    {
        // Basic movement input.
        inputVelocity = new Vector2(Input.GetAxis("Horizontal") * movementVelocity, Input.GetAxis("Vertical") * movementVelocity);

        // Running input.
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputVelocity = new Vector2(inputVelocity.x * runningVelocityMultiplier, inputVelocity.y * runningVelocityMultiplier);
        }


        playerRB.velocity = Vector2.Lerp(playerRB.velocity, inputVelocity, Time.deltaTime * movementVelocity);

        playerAnimator.SetBool("Walking", playerRB.velocity != Vector2.zero);
        playerAnimator.SetBool("Running", (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)));

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
