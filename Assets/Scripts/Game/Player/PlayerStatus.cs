using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script will control all non-item resources for the player, aka health, energy (?) etc.
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    [Header("Movement")]
    public float movementVelocity = 5f;
    public float runningVelocityMultiplier = 2f;
    public float staminaRechargeVelocity = 3f;
    public float staminaDrainVelocity = 10f;
    public float staminaRechargeTimer = 3f;
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

    private List<ActiveBuffs> activeBuffs;

    private void Start()
    {
        activeBuffs = new List<ActiveBuffs>();

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
        currentHealth = Mathf.Clamp(currentHealth + amount, -0.5f, maxHealth);
        healthSlider.value = currentHealth;

        if (amount < 0) playerDamageAnimator.SetTrigger("Take Damage");

        if (currentHealth <= 0f) FindObjectOfType<Death>().Trigger();
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

    /// <summary>
    /// Applies a buff on the player.
    /// </summary>
    /// <param name="buff">The target buff.</param>
    public void ApplyBuff(Buff buff)
    {
        Debug.Log(activeBuffs.Count);
        ActiveBuffs activeBuff = activeBuffs.Find((x) => x.buff == buff);

        float timer = 0f;
        if (buff.useFixedTime) timer = buff.fixedTime;
        else timer = Random.Range(buff.minTimer, buff.maxTimer);

        if (activeBuff != null)
        {
            activeBuff.timer = timer;
            return;
        }

        activeBuffs.Add(new ActiveBuffs(buff, timer));

        ActiveBuffs added = activeBuffs[activeBuffs.Count - 1];
        added.coroutine = StartCoroutine(BuffCycle(added));

        if (added.buff.specialParticles)
        {
            added.spawnedParticles = Instantiate(added.buff.specialParticles, transform);
            added.spawnedParticles.localPosition = Vector2.zero;
        }
    }

    private IEnumerator BuffCycle(ActiveBuffs buff)
    {
        float buffSpecialLogicTick = 0.5f;

        while (buff.timer >= 0f)
        {
            buff.timer -= Time.deltaTime;

            buffSpecialLogicTick -= Time.deltaTime;
            if (buffSpecialLogicTick <= 0f)
            {
                RunBuffSpecialLogic(buff.buff);
                buffSpecialLogicTick = 0.5f;
            }

            yield return null;
        }

        if (buff.spawnedParticles) Destroy(buff.spawnedParticles.gameObject);
        activeBuffs.Remove(buff);
    }

    /// <summary>
    /// This function will take care of special behaviour buffs, like poisons, etc
    /// </summary>
    private void RunBuffSpecialLogic(Buff buff)
    {
        switch (buff.type)
        {
            case Buff.Type.Poison:
                UpdateHealth(-10f);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Calculates the movement velocity based on all debuffs.
    /// </summary>
    /// <returns>The final velocity.</returns>
    public float GetVelocity()
    {
        float finalVelocity = movementVelocity;

        foreach (ActiveBuffs item in activeBuffs)
        {
            if (item.buff.type == Buff.Type.Slowness) finalVelocity *= 0.8f;
        }

        return finalVelocity;
    }

    /// <summary>
    /// Calculates the movement velocity run multiplier based on all debuffs.
    /// </summary>
    /// <returns>The final multiplier.</returns>
    public float GetRunVelocityMultiplier()
    {
        float finalMultiplier = runningVelocityMultiplier;

        foreach (ActiveBuffs item in activeBuffs)
        {
            if (item.buff.type == Buff.Type.Slowness) finalMultiplier *= 0.8f;
        }

        return finalMultiplier;
    }

    /// <summary>
    /// Calculates the stamina drain velocity based on all debuffs.
    /// </summary>
    /// <returns>The final velocity.</returns>
    public float GetStaminaDrainVelocity()
    {
        return staminaDrainVelocity;
    }

    /// <summary>
    /// Calculates the stamina charge timer based on all debuffs.
    /// </summary>
    /// <returns>The final timer.</returns>
    public float GetStaminaChargeTimer()
    {
        return staminaRechargeTimer;
    }

    /// <summary>
    /// Calculates the stamina recharge velocity based on all debuffs.
    /// </summary>
    /// <returns>The final velocity.</returns>
    public float GetStaminaRechargeVelocity()
    {
        return staminaRechargeVelocity;
    }
}
