using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script controls the enemy's status, like health, special meters, etc.
/// </summary>
public class EnemyStatus : MonoBehaviour
{
    [Header("Health")]
    public float health;
    [Header("Defenses")]
    public float damageReduction;
    [Header("Offensive")]
    public float damage;
    [Header("References")]
    public Transform worldHealthbarPrefab;
    public Transform worldHealthbarHolder;

    private float healthbarTimer;
    private Transform currentHealthbar;
    private float currentHealth;
    private Animator damageAnimator;

    private void Start()
    {
        damageAnimator = transform.Find("Skin").GetComponent<Animator>();
        worldHealthbarHolder = GameObject.Find("Health Bars Canvas").transform;
        currentHealth = health;
    }

    private void Update()
    {
        if (healthbarTimer > 0f)
        {
            if (!currentHealthbar) currentHealthbar = Instantiate(worldHealthbarPrefab, worldHealthbarHolder);

            Slider healthbar = currentHealthbar.GetComponent<Slider>();
            healthbar.maxValue = health;
            healthbar.value = currentHealth;

            currentHealthbar.position = new Vector2(transform.position.x, transform.position.y + 1.5f);

            healthbarTimer -= Time.deltaTime;
        }
        else if (currentHealthbar) Destroy(currentHealthbar.gameObject);
    }

    /// <summary>
    /// Changes the current amount of health this enemy has.
    /// </summary>
    /// <param name="amount">The target amount.</param>
    public void UpdateHealth(float amount)
    {
        currentHealth += (amount - ((amount / 100f) * damageReduction));

        if (amount < 0f) damageAnimator.SetTrigger("Take Damage");

        if (currentHealth <= 0f) 
        {
            FindObjectOfType<EnemySpawner>().spawnedEnemies.Remove(this.transform);
            if (currentHealthbar) Destroy(currentHealthbar.gameObject);
            Destroy(this.gameObject); 
        }

        healthbarTimer = 3f;
    }
}
