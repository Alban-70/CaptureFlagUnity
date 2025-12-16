using UnityEngine;

public class TakeCoins : MonoBehaviour
{
    
    [SerializeField] private GameManager gameManager;
    [SerializeField] private LayerMask playerLayer;


    void OnTriggerEnter(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & playerLayer) != 0)
        {
            gameManager.getCoin = true;
            Destroy(gameObject);
        }
    }
}
