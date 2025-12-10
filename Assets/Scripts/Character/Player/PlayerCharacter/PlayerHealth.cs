using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private GameObject playerCamera;

    private HealthSystem healthSystem;

    void Awake()
    {
        // Récupère le HealthSystem sur le joueur
        healthSystem = GetComponent<HealthSystem>();

        // Initialise la caméra
        playerCamera = transform.GetChild(0).GetChild(0).gameObject;
    }

    void Update()
    {
        if (healthSystem.IsDead())
        {
            // Détache la caméra avant de mourir
            playerCamera.transform.SetParent(null);
            Die();
        }
    }

    // public void ApplyDamage(float damage)
    // {
    //     healthSystem.ApplyDamage(damage);

    //     if (healthSystem.IsDead())
    //     {
    //         // Détache la caméra avant de mourir
    //         playerCamera.transform.SetParent(null);
    //         Die();
    //     }
    // }

    private void Die()
    {
        Debug.Log("Player is dead");
        Destroy(gameObject);
    }
}
