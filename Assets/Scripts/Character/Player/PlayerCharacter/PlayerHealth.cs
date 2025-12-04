using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Damage Settings")]
    private string damageSourceLayer = "Enemy"; // On ne prend des dégâts que des objets de ce layer

    private float maxHealth = 100f; // La vie maximale du joueur
    private float currentHealth; // La vie actuelle
    private int damageSourceLayerInt; // Valeur du layer convertie en int
    private GameObject playerCamera; // Référence à la caméra du joueur (pour la détacher à la mort)

    /// <summary>
    /// Initialise la vie du joueur et récupère la caméra principale.
    /// </summary>
    void Awake()
    {
        // On récupère la caméra principale du joueur
        playerCamera = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;

        currentHealth = maxHealth; // On commence avec la vie max
        damageSourceLayerInt = LayerMask.NameToLayer(damageSourceLayer); // Convertit le nom du layer en int
    }

    /// <summary>
    /// Méthode appelée chaque frame. Actuellement vide mais peut être utilisée pour des mises à jour de santé ou effets.
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// Gère la mort du joueur en affichant un message et en détruisant le GameObject.
    /// </summary>
    private void Die()
    {
        Debug.Log("Player is dead");
        Destroy(gameObject);
    }

    /// <summary>
    /// Applique des dégâts au joueur et déclenche la mort si la santé tombe à zéro ou moins.
    /// </summary>
    /// <param name="damage">Montant de dégâts à appliquer au joueur.</param>
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
