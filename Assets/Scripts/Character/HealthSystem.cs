using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator pretreAnimator;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;

    public float maxHealth;
    public float currentHealth;

    private PlayerMovement playerMovement;
    private bool isDead = false;

    void Awake()
    {
        if (gameObject.tag == "Player")
            playerMovement = GetComponent<PlayerMovement>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!isDead && IsDead())
        {
            if (gameObject.CompareTag("Player") && playerCamera != null)
            {
                PlayerDeath();
            }

            else 
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

    private void PlayerDeath()
    {
        isDead = true;

        playerCamera.transform.SetParent(null);
        if (playerAnimator != null)
            playerAnimator.SetTrigger("Death");

        audioManager.PlayMort();
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    public void PlayerWin()
    {
        if (pretreAnimator != null)
            pretreAnimator.SetTrigger("Applause");

        audioManager.PlayWin();
    }

    public void ShowGameWinning()
    {
        gameManager.ShowWinning();
    }

    public void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (gameManager != null)
                gameManager.ShowGameOver();
        }
        Destroy(gameObject);
    }
    
}
