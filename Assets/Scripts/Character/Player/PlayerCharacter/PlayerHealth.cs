using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f; // La vie maximale du joueur

    [Header("Damage Settings")]
    private string damageSourceLayer = "Enemy"; // On ne prend des dégâts que des objets de ce layer

    private float currentHealth; // La vie actuelle
    private int damageSourceLayerInt;
    private GameObject playerCamera; // Référence à la caméra du joueur (pour la détacher à la mort)

    void Awake()
    {
        // On récupère la caméra principale du joueur
        playerCamera = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;

        currentHealth = maxHealth; // On commence avec la vie max
        damageSourceLayerInt = LayerMask.NameToLayer(damageSourceLayer); // Convertit le nom du layer en int
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

    public void ApplyDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log("Health = " + currentHealth);

        if (currentHealth <= 0f)
        {
            // On détache la caméra avant de détruire le joueur
            playerCamera.transform.SetParent(null);
            Die();
        }
    }
}
