using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private string damageSourceLayer = "Player"; 

    private float maxHealth = 150f;
    private float currentHealth;
    private int damageSourceLayerInt;

    /// <summary>
    /// Initialise la santé de l'ennemi et configure le layer de la source de dégâts.
    /// </summary>
    void Awake()
    {
        currentHealth = maxHealth;
        damageSourceLayerInt = LayerMask.NameToLayer(damageSourceLayer);
    }

    /// <summary>
    /// Méthode appelée à chaque frame.
    /// Actuellement vide mais peut être utilisée pour mettre à jour des comportements.
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// Gère la mort de l'ennemi en affichant un message et en détruisant l'objet.
    /// </summary>
    private void Die()
    {
        Debug.Log("Enemy is dead");
        Destroy(gameObject);
    }

    /// <summary>
    /// Applique des dégâts à l'ennemi et déclenche la mort si la santé tombe à zéro ou moins.
    /// </summary>
    /// <param name="damage">Montant de dégâts à appliquer à l'ennemi.</param>
    private void ApplyDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Health = " + currentHealth);

        if (currentHealth <= 0f)
            Die();
    }
}
