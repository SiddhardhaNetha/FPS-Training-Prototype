using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    private Renderer targetRenderer;
    private Color originalColor;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        // Remember where the target started
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
        {
            originalColor = targetRenderer.material.color;
        }

        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;

        Debug.Log(
            "Target Dummy hit! Damage: " + damage +
            " | HP: " + currentHealth
        );

        HitResponse();

        if (currentHealth <= 0)
        {
            DestroyTarget();
        }
    }

    private void HitResponse()
    {
        if (targetRenderer == null)
            return;

        targetRenderer.material.color = Color.red;

        CancelInvoke(nameof(ResetColor));
        Invoke(nameof(ResetColor), 0.1f);
    }

    private void ResetColor()
    {
        if (targetRenderer != null)
        {
            targetRenderer.material.color = originalColor;
        }
    }

    private void DestroyTarget()
    {
        currentHealth = 0;

        Debug.Log("Target Dummy destroyed!");

        gameObject.SetActive(false);
    }

    public void ResetTarget()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        currentHealth = maxHealth;

        if (targetRenderer != null)
        {
            targetRenderer.material.color = originalColor;
        }

        gameObject.SetActive(true);

        Debug.Log("Target Dummy reset!");
    }
}