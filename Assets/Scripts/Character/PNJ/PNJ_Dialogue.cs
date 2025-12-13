using TMPro;
using UnityEngine;

public class PNJ_Dialogue : MonoBehaviour
{
    [SerializeField] private Animator animPNJ;
    [SerializeField] private TextMeshProUGUI textSpeak; 
    [SerializeField] private PlayerInputs playerInputs;
    [SerializeField] private LayerMask playerLayer;
    [TextArea(3, 5)] 
    [SerializeField] public string[] dialogues;

    [HideInInspector] public bool showText = false;

    void Awake()
    {
        textSpeak.text = $"Appuyez sur [{playerInputs.GetDialogKey()}] pour parler";
    }

    void Update()
    {
        textSpeak.gameObject.SetActive(showText);
    }

    public void StartSpeakAnimation()
    {
        animPNJ.SetTrigger("Speaking");
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
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
            showText = false;
            PlayerDialogue playerDialogue = other.GetComponentInParent<PlayerDialogue>();
            if (playerDialogue != null)
            {
                playerDialogue.ClearPNJ();
            }
        }
    }

}
