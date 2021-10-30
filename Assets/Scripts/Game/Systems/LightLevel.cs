using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controlls the light level of the world.
/// Will also update the display.
/// </summary>
public class LightLevel : MonoBehaviour
{
    [Header("Data")]
    public bool isCharging;
    public float maxLight;
    public float currentLight;
    public float decayVelocity;
    public float rechargeVelocity;
    [Header("References")]
    public LightLevelDisplay display;

    private void Update()
    {
        if (isCharging)
        {
            currentLight += Time.deltaTime * rechargeVelocity;
            currentLight = Mathf.Clamp(currentLight, 0f, maxLight);

            display.UpdateLightLevel(maxLight, currentLight, true);

            if (currentLight >= maxLight) { isCharging = false; }
            return;
        }

        currentLight -= Time.deltaTime * decayVelocity;
        currentLight = Mathf.Clamp(currentLight, 0f, maxLight);

        display.UpdateLightLevel(maxLight, currentLight);

        if (currentLight <= 0f) { isCharging = true; }
    }
}
