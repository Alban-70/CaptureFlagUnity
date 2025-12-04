using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Damage Settings")]
    // [SerializeField] private float damageOnHit = 10f;
    [SerializeField] private string damageSourceLayer = "Player"; 


    private float currentHealth;
    private int damageSourceLayerInt;

    void Awake()
    {
        currentHealth = maxHealth;
        damageSourceLayerInt = LayerMask.NameToLayer(damageSourceLayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die()
    {
        Debug.Log("Enemy is dead");
        Destroy(gameObject);
    }

    private void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Health = " + currentHealth);

        if (currentHealth <= 0f)
            Die();
    }
}
