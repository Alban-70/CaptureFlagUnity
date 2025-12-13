using UnityEngine;

public class PNJ_Dialogue : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [TextArea(3, 5)] 
    [SerializeField] public string[] dialogues;

    void Awake()
    {
        Debug.Log("marchand");
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            
            PlayerDialogue playerDialogue = other.GetComponentInParent<PlayerDialogue>();
            if (playerDialogue != null)
            {
                playerDialogue.SetCurrentPNJ(this);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            PlayerDialogue playerDialogue = other.GetComponentInParent<PlayerDialogue>();
            if (playerDialogue != null)
            {
                playerDialogue.ClearPNJ();
            }
        }
    }

}
