using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    [Header("Drops")]
    public LootTable drops;
    [Header("References")]
    public Transform worldHealthbarPrefab;
    public Transform worldHealthbarHolder;

    private float healthbarTimer = 0f;
    private Transform currentHealthbar;

    private void Start()
    {
        worldHealthbarHolder = GameObject.Find("Health Bars Canvas").transform;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (healthbarTimer > 0f)
        {
            if (!currentHealthbar) currentHealthbar = Instantiate(worldHealthbarPrefab, worldHealthbarHolder);

            Slider healthbar = currentHealthbar.GetComponent<Slider>();
            healthbar.maxValue = maxHealth;
            healthbar.value = currentHealth;

            currentHealthbar.position = new Vector2(transform.position.x, transform.position.y - 0.5f);

            healthbarTimer -= Time.deltaTime;
        }
        else if (currentHealthbar) Destroy(currentHealthbar.gameObject);
    }

    /// <summary>
    /// Changes the health by a given amount.
    /// </summary>
    /// <param name="amount">The amount.</param>
    public void UpdateHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        healthbarTimer = 3f;

        if (currentHealth <= 0f)
        {
            FindObjectOfType<Inventory>().DropLootTable(drops, transform.position);
            if (currentHealthbar) Destroy(currentHealthbar.gameObject);
            Destroy(this.transform.parent.gameObject);
        }
    }
}
