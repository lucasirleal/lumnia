using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script controls the light level display on the screen.
/// Does not change the light level.
/// </summary>
public class LightLevelDisplay : MonoBehaviour
{
    [Header("Settings")]
    public Gradient lightLevelWarning;
    public Color rechargingColor;
    [Header("References")]
    public Slider lightLevelSlider;
    public Image lightLevelFill;

    /// <summary>
    /// Updates the light level display.
    /// </summary>
    public void UpdateLightLevel(float maxValue, float currentValue, bool isCharging = false)
    {
        lightLevelSlider.maxValue = maxValue;
        lightLevelSlider.value = currentValue;

        if (isCharging)
        {
            lightLevelFill.color = rechargingColor;
            return;
        }

        lightLevelFill.color = lightLevelWarning.Evaluate(currentValue / maxValue);
    }
}
