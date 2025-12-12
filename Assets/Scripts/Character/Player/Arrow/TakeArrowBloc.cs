using TMPro;
using UnityEngine;

public class TakeArrowBloc : MonoBehaviour
{

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private TextMeshProUGUI takeArrowsText;
    [SerializeField] private PlayerCombat playerCombat;


    private bool showCanva = false;


    void Update()
    {
        takeArrowsText.gameObject.SetActive(showCanva);
        
        if (showCanva && Input.GetKeyDown(KeyCode.E))
        {
            playerCombat.AddArrows(5);
            Debug.Log("ajout de 5 flèches");
            // Ajouter le nombre de flèches : +5
            takeArrowsText.gameObject.SetActive(false);
            Destroy(gameObject);
            
        }
    }

    public void ChildTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Debug.Log("enter");
            showCanva = true;
        }
    }

    public void ChildTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Debug.Log("enter");
            showCanva = false;
        }
    }
    
}
