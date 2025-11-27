using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Damage Settings")]
    [SerializeField] private float damageOnHit = 10f;
    private string damageSourceLayer = "Enemy"; 

    private float currentHealth;
    private int damageSourceLayerInt;
    private GameObject playerCamera;

    void Awake()
    {
        playerCamera = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
        currentHealth = maxHealth;
        damageSourceLayerInt = LayerMask.NameToLayer(damageSourceLayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die()
    {
        Debug.Log("Player is dead");
        Destroy(gameObject);
    }

    private void ApplyDamage(float damage)
    {
        // currentHealth -= damage;
        Debug.Log("Health = " + currentHealth);

        if (currentHealth <= 0f)
        {
            playerCamera.transform.SetParent(null);
            Die();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer != damageSourceLayerInt)
            return;

        ApplyDamage(damageOnHit);
    }
}
