using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script will control all non-item resources for the player, aka health, energy (?) etc.
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public Slider healthSlider;
    public float currentHealth;
    [Header("Stamina")]
    public float maxStamina;
    public Slider staminaSlider;
    public Color rechargeColor;
    public Color originalColor;
    public float currentStamina;
    [Header("References")]
    public Animator playerDamageAnimator;

    private void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    /// <summary>
    /// Changes the health by a given amount.
    /// </summary>
    /// <param name="amount">The amount.</param>
    public void UpdateHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthSlider.value = currentHealth;

        if (amount < 0) playerDamageAnimator.SetTrigger("Take Damage");
    }

    /// <summary>
    /// Changes the stamina by a given amount.
    /// </summary>
    /// <param name="amount">The amount.</param>
    public void UpdateStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
        staminaSlider.value = currentStamina;
    }

    /// <summary>
    /// Updates the stamina recharging state. (The color of the slider.)
    /// </summary>
    /// <param name="state">Send true if it is charging.</param>
    public void UpdateStaminaCharging(bool state)
    {
        if (state) { staminaSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = rechargeColor; } 
        else { staminaSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = originalColor; }
    }
}
