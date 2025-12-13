using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{

    [SerializeField] private PlayerInputs playerInputs;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Canvas dialogCanvas;
    [SerializeField] private TextMeshProUGUI showTextForSpeak;
    [SerializeField] private TextMeshProUGUI textUI;
    

    private PNJ_Dialogue currentPNJ;
    private string[] currentDialogues;

    private float rotationSpeed = 5f;
    private float letterDelay = 0.05f;
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isDialoging = false;
    [HideInInspector] public bool canStartQuest = false;
    private Coroutine typingCoroutine;


    void Awake()
    {
        dialogCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (currentPNJ != null && !isDialoging && playerInputs.IsDialog())
        {
            Debug.Log("dialog en cours");
            StartDialogue(currentPNJ);
        }

        if (playerInputs.IsNextDialog() && isDialoging)
            NextDialogue();

    }

    void StartDialogue(PNJ_Dialogue pnj)
    {
        currentPNJ.StartSpeakAnimation();
        currentPNJ.showText = false;
        StartCoroutine(RotatePNJToPlayer(pnj.transform));

        dialogCanvas.gameObject.SetActive(true);
        playerMovement.canMove = false;
        playerMovement.canJump = false;

        currentDialogues = pnj.dialogues;
        currentIndex = 0;
        isDialoging = true;

        typingCoroutine = StartCoroutine(TypeText(currentDialogues[currentIndex]));
    }
    

    void NextDialogue()
    {
        if (currentPNJ == null)
            return;
        
        currentPNJ.StartSpeakAnimation();
        if (currentDialogues == null || currentDialogues.Length == 0)
            return;

        // Si on est en train d’écrire → on affiche tout direct
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            textUI.text = currentDialogues[currentIndex];
            isTyping = false;
            return;
        }

        // Phrase suivante
        currentIndex++;

        if (currentIndex < currentDialogues.Length)
        {
            typingCoroutine = StartCoroutine(TypeText(currentDialogues[currentIndex]));
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        currentPNJ.StopSpeakAnimation();
        // Fin du dialogue
        dialogCanvas.gameObject.SetActive(false);
        isDialoging = false;
        currentDialogues = null;

        playerMovement.canMove = true;
        playerMovement.canJump = true;

        if (currentPNJ.tag == "Pretre")
            canStartQuest = true;
    }

    public void SetCurrentPNJ(PNJ_Dialogue pnj)
    {   
        currentPNJ = pnj;
        showTextForSpeak.text = $"Appuyez sur [{currentPNJ.playerInputs.GetDialogKey()}] pour parler";
        showTextForSpeak.gameObject.SetActive(true);
    }

    public void ClearPNJ()
    {
        currentPNJ = null;
        showTextForSpeak.gameObject.SetActive(false);
    }

    IEnumerator TypeText(string sentence)
    {
        isTyping = true;
        textUI.text = "";

        foreach (char letter in sentence)
        {
            textUI.text += letter;
            float delay = letterDelay;

            if (letter == ',' || letter == ';' || letter == ':')
                delay = 0.5f;

            else if (letter == '.' || letter == '!' || letter == '?')
                delay = 0.8f;

            yield return new WaitForSecondsRealtime(delay);
        }

        isTyping = false;
    }


    IEnumerator RotatePNJToPlayer(Transform pnjTransform)
    {
        Vector3 direction = transform.position - pnjTransform.position;
        direction.y = 0f;

        if (direction == Vector3.zero)  // Si le PNJ regarde déjà le player
            yield break;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (Quaternion.Angle(pnjTransform.rotation, targetRotation) > 0.5f)
        {
            pnjTransform.rotation = Quaternion.Slerp(
                pnjTransform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );

            yield return null;
        }

        pnjTransform.rotation = targetRotation;
    }
}