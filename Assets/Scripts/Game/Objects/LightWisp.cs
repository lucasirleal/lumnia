using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the light wisp around the player.
/// </summary>
public class LightWisp : MonoBehaviour
{
    [Header("References")]
    public LightLevel lightSystem;
    public Transform player;

    private Vector2 offset = new Vector2(1f, 1f);
    private float offsetChangeTimer = 5f;

    // Update is called once per frame
    void Update()
    {
        if (lightSystem.currentLight <= 0f || lightSystem.isCharging) { transform.GetChild(0).gameObject.SetActive(false); }
        else { transform.GetChild(0).gameObject.SetActive(true); }

        offsetChangeTimer -= Time.deltaTime;
        if (offsetChangeTimer <= 0f)
        {
            offset = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            offsetChangeTimer = Random.Range(1f, 3f);
        }

        transform.position = Vector2.Lerp(transform.position, (Vector2)player.position + offset, Time.deltaTime * 3f);
    }
}
