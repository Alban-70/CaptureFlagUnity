using UnityEngine;

public class TakeCoins : MonoBehaviour
{
    
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UI_Manager uI_Manager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private LayerMask playerLayer;


    void OnTriggerEnter(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & playerLayer) != 0)
        {
            gameManager.getCoin = true;
            uI_Manager.textQuest.text = "Livrer le trésor";
            audioManager.PlayGetCoin();
            Destroy(gameObject);
        }
    }
}
