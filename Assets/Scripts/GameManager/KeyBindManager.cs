using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class KeyBindManager : MonoBehaviour
{

    [SerializeField] private PauseManager pauseManager;

    
    [Serializable]
    public class KeybindButton
    {
        public string actionName;
        public TextMeshProUGUI buttonText;
    }

    
    [SerializeField] private List<KeybindButton> keybindButtons;

    [SerializeField] private PlayerInputs playerInputs;


    // Rebind System
    private bool waitingForKey = false;
    private string currentActionToRebind;
    private TextMeshProUGUI currentButtonText;
    private Dictionary<string, Action<KeyCode>> keyActions;


    void Awake()
    {
        UpdateAllKeybindTexts();
        keyActions = new Dictionary<string, Action<KeyCode>>()
        {
            {"Avancer", k => playerInputs.moveForwardKey = k },
            {"Reculer", k => playerInputs.moveBackwardKey = k },
            {"Droite", k => playerInputs.moveRightKey = k },
            {"Gauche", k => playerInputs.moveLeftKey = k },
            {"Sauter", k => playerInputs.jumpKey = k },
            {"AttaquerEpee", k => playerInputs.attackKey = k },
            {"Viser", k => playerInputs.bowKey = k },
            {"AttaquerArc", k => playerInputs.bowShootKey = k },
            {"SwitchSword", k => playerInputs.switchSwordKey = k },
            {"SwitchBow", k => playerInputs.switchBowKey = k },
            {"Courir", k => playerInputs.runKey = k },
            {"Pause", k => playerInputs.pauseKey = k },
            {"PrendreItems", k => playerInputs.getItemsKey = k}
        };
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(currentActionToRebind) && Input.anyKeyDown)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    AssignKey(currentActionToRebind, key);
                    currentActionToRebind = "";
                    break;
                }
            }
        }

        if (playerInputs.IsStoppingGame())
            pauseManager.TogglePause();
    }

    public void StartRebind(string actionName)
    {
        if (waitingForKey) return; // ignore si déjà en attente
        currentActionToRebind = actionName;

        KeybindButton kb = keybindButtons.Find(k => k.actionName == actionName);
        if (kb != null)
        {
            currentButtonText = kb.buttonText;
            currentButtonText.text = "> " + currentButtonText.text + " <";
        }
    } 

    private void AssignKey(string actionName, KeyCode newKey)
    {
        if (keyActions.TryGetValue(actionName, out var assign))
            assign(newKey);

        // Met à jour le texte automatiquement
        foreach (var kb in keybindButtons)
            if (kb.actionName == actionName)
                kb.buttonText.text = newKey.ToString();
    }

    public void OnRebindButtonClicked(string actionName)
    {
        StartRebind(actionName);
    }

    private void UpdateAllKeybindTexts()
    {
        foreach (var kb in keybindButtons)
        {
            switch (kb.actionName)
            {
                case "Avancer":
                    kb.buttonText.text = playerInputs.GetMoveForwardKey().ToString();
                    break;
                case "Reculer":
                    kb.buttonText.text = playerInputs.GetMoveBackwardKey().ToString();
                    break;
                case "Droite":
                    kb.buttonText.text = playerInputs.GetMoveRightKey().ToString();
                    break;
                case "Gauche":
                    kb.buttonText.text = playerInputs.GetMoveLeftKey().ToString();
                    break;
                case "Sauter":
                    kb.buttonText.text = playerInputs.GetJumpKey().ToString();
                    break;
                case "AttaquerEpee":
                    kb.buttonText.text = KeyCodeForUser(playerInputs.GetAttackKey());
                    break;
                case "Viser":
                    kb.buttonText.text = KeyCodeForUser(playerInputs.GetBowKey());
                    break;
                case "AttaquerArc":
                    kb.buttonText.text = KeyCodeForUser(playerInputs.GetBowShootKey());
                    break;
                case "SwitchSword":
                    kb.buttonText.text = playerInputs.GetSwitchSwordKey().ToString();
                    break;
                case "SwitchBow":
                    kb.buttonText.text = playerInputs.GetSwitchBowKey().ToString();
                    break;
                case "Courir":
                    kb.buttonText.text = KeyCodeForUser(playerInputs.GetRunKey());
                    break;
                case "Pause":
                    kb.buttonText.text = playerInputs.GetPauseKey().ToString();
                    break;
                case "PrendreItems":
                    kb.buttonText.text = playerInputs.GetItemsKey().ToString();
                    break;
            }
        }
    }

    private string KeyCodeForUser(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Mouse0: return "Clic gauche";
            case KeyCode.Mouse1: return "Clic droit";
            case KeyCode.Mouse2: return "Clic molette";
            case KeyCode.Mouse3: return "Bouton souris 4";
            case KeyCode.Mouse4: return "Bouton souris 5";
            case KeyCode.Mouse5: return "Bouton souris 6";
            case KeyCode.Mouse6: return "Bouton souris 7";

            case KeyCode.LeftShift: return "Shift gauche";
            case KeyCode.RightShift: return "Shift droit";
            case KeyCode.LeftControl: return "Ctrl gauche";
            case KeyCode.RightControl: return "Ctrl droit";
            case KeyCode.LeftAlt: return "Alt gauche";
            case KeyCode.RightAlt: return "Alt droit";

            default:
                return key.ToString();
        }
    }



    
}
