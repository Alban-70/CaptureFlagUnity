using System.Collections;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    #region Unity Methods
    /// <summary>
    /// Méthode appelée lors de l'initialisation du script.
    /// Actuellement vide mais peut être utilisée pour initialiser des valeurs.
    /// </summary>
    void Awake()
    {
        
    }

    /// <summary>
    /// Méthode appelée lorsqu'une collision se produit.
    /// Gère la logique de contact avec les ennemis et détruit la flèche après collision.
    /// </summary>
    /// <param name="collision">L'objet Collision qui contient des informations sur la collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        // Ignore si c'est le joueur
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();

            // Si c'est un ennemi
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Touché");
                
                if (enemyRb != null)
                {
                    // Gèle la position et la rotation de l'ennemi
                    enemyRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

                    // Démarre la coroutine pour libérer le mouvement après un délai
                    StartCoroutine(UnFreezeAfterDelay(enemyRb, 0.5f));
                }
            }

            // Détruit la flèche après la collision
            Destroy(gameObject);
        }
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Libère les contraintes de position de l'ennemi après un délai donné.
    /// </summary>
    /// <param name="enemyRb">Le Rigidbody de l'ennemi à débloquer.</param>
    /// <param name="delay">Le temps en secondes avant de libérer les contraintes.</param>
    /// <returns>Coroutine IEnumerator pour le StartCoroutine.</returns>
    private IEnumerator UnFreezeAfterDelay(Rigidbody enemyRb, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemyRb != null)
            enemyRb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    #endregion
}
