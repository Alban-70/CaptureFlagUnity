using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerDialogue : MonoBehaviour
{

    [SerializeField] private PlayerInputs playerInputs;
    [SerializeField] private Canvas dialogCanvas;
    [SerializeField] private TextMeshProUGUI textUI;
    

    private PNJ_Dialogue currentPNJ;
    private string[] currentDialogues;

    private float letterDelay = 0.05f;
    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isDialoging = false;
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
        dialogCanvas.gameObject.SetActive(true);
        currentDialogues = pnj.dialogues;
        currentIndex = 0;
        isDialoging = true;

        typingCoroutine = StartCoroutine(TypeText(currentDialogues[currentIndex]));
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

    void NextDialogue()
    {
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
        // Fin du dialogue
        dialogCanvas.gameObject.SetActive(false);
        isDialoging = false;
        currentDialogues = null;
        currentPNJ = null;
    }

    public void SetCurrentPNJ(PNJ_Dialogue pnj)
    {   
        currentPNJ = pnj;
    }

    public void ClearPNJ()
    {
        currentPNJ = null;
    }

}