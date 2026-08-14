using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public TMP_Text healthText;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maxHealth
        );

        UpdateHealthUI();

        Debug.Log("Player HP: " + currentHealth);
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text =
                "HP: " + currentHealth.ToString("0");
        }
    }
}