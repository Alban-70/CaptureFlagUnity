using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;

    public float maxHealth;
    public float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (IsDead())
        {
            if (gameObject.CompareTag("Player") && playerCamera != null)
                playerCamera.transform.SetParent(null);
            Die();
        }
    }

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("enemy touché");
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public bool IsDead()
    {
        return currentHealth <= 0f;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
