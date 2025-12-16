using TMPro;
using UnityEngine;

public class PNJ_Dialogue : MonoBehaviour
{
    [SerializeField] private Animator animPNJ;
    [SerializeField] public PlayerInputs playerInputs;
    [SerializeField] private LayerMask playerLayer;
    [TextArea(3, 5)] 

    [HideInInspector] public bool showText = false;

    [TextArea(3, 5)] 
    public string[] dialogues;
    [TextArea(3, 5)] 
    public string[] dialogueVictoire;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void StartSpeakAnimation()
    {
        animPNJ.SetTrigger("Speaking");
    }

    public void StopSpeakAnimation()
    {
        animPNJ.ResetTrigger("Speaking");
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            rb.isKinematic = true;
            showText = true;
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
            rb.isKinematic = false;
            showText = false;
            PlayerDialogue playerDialogue = other.GetComponentInParent<PlayerDialogue>();
            if (playerDialogue != null)
            {
                playerDialogue.ClearPNJ();
            }
        }
    }

}
