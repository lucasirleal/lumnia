using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script will basically make anything follow the player.
/// When a certain distance threshold has been reached, it will fire a callback function.
/// After that, the script will be locked in place, and will wait for another script to call the unlock function.
/// </summary>
public class FollowAI : MonoBehaviour
{
    [Header("Settings")]
    public float distanceTreshold;
    public float maxDistanceToFollow;
    public float walkVelocity;
    public bool isStaggered;
    public UnityEvent callback;
    [Header("References")]
    public Transform player;
    public Rigidbody2D actorRB;
    public Animator actorAnimator;

    private Vector2 inputVelocity;
    private float currentDistance;
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Follow");
    }

    private void FixedUpdate()
    {
        actorRB.velocity = Vector2.Lerp(actorRB.velocity, inputVelocity, Time.deltaTime * 5f);
        actorAnimator.SetBool("Walking", actorRB.velocity != Vector2.zero);
    }

    private void Update()
    {
        // If the actor is staggered due to a bigger distance from the player,
        // we'll wait untill the player goes visible again.
        if (!isStaggered) return;
        UpdateCurrentDistance();
        if (currentDistance < maxDistanceToFollow)
        {
            StartCoroutine("Follow");
        }
    }

    /// <summary>
    /// Calculates again the current distance from the player.
    /// </summary>
    private void UpdateCurrentDistance()
    {
        currentDistance = Vector2.Distance(transform.position, player.position);
    }

    /// <summary>
    /// Calculates the force necessary to walk towards the player.
    /// </summary>
    /// <returns></returns>
    private Vector2 CalculateForceTowards()
    {
        return (player.position - transform.position).normalized;
    }

    /// <summary>
    /// Makes it so the actor can follow again.
    /// </summary>
    public void UnlockActor()
    {
        StartCoroutine("Follow");
    }

    private IEnumerator Follow()
    {
        isStaggered = false;
        UpdateCurrentDistance();

        while (currentDistance <= maxDistanceToFollow && currentDistance >= distanceTreshold)
        {
            inputVelocity = CalculateForceTowards() * walkVelocity;
            UpdateCurrentDistance();
            yield return null;
        }

        inputVelocity = Vector2.zero;

        if (currentDistance > maxDistanceToFollow)
        {
            isStaggered = true;
        }
        else if (currentDistance < distanceTreshold)
        {
            callback.Invoke();
        }
    }
}
