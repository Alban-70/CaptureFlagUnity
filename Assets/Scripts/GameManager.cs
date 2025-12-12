using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Serializable]
    public class KeybindButton
    {
        public string actionName;
        public TextMeshProUGUI buttonText;
    }
    [SerializeField] private List<KeybindButton> keybindButtons;

    [SerializeField] private PlayerInputs playerInputs;

    [Header("UI References")]
    [SerializeField] private Canvas startMenu;
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private CanvasGroup gameOverPanel;
    [SerializeField] private Sprite customCursor;
    [SerializeField] private AudioClip[] audioClipsVolume;
    [SerializeField] private Slider sliderVolume; 
    [SerializeField] private Slider sliderMusic; 
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI volumeText;


    [Header("Basic color")]
    [SerializeField] private Color baseColorText;

    [Header("Differents Blocs de settings")]
    [SerializeField] private GameObject blocGame;
    [SerializeField] private GameObject blocGraphics;
    [SerializeField] private GameObject blocSounds;
    [SerializeField] private GameObject blocKeybinds;
    
    
    private AudioSource audioSource;
    private Texture2D cursorTexture;
    private float cursorScale = 0.5f;
    private bool isPaused = false;
    private bool isGameOver = false;
    private float fadeDuration = 5f;
    private float lastVolume = -1f;
    private float lastMusic = -1f;

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
            {"Attaquer", k => playerInputs.attackKey = k },
            {"Arc", k => playerInputs.bowKey = k },
            {"SwitchSword", k => playerInputs.switchSwordKey = k },
            {"SwitchBow", k => playerInputs.switchBowKey = k },
            {"Courir", k => playerInputs.runKey = k },
            {"Pause", k => playerInputs.pauseKey = k }
        };

        audioSource = GetComponent<AudioSource>();

        if (startMenu != null)
            startMenu.gameObject.SetActive(true);

        if (pauseCanvas != null)
            pauseCanvas.gameObject.SetActive(false);

        Time.timeScale = 0f;

        cursorTexture = GetTextureFromSprite(customCursor);
        int newW = Mathf.RoundToInt(cursorTexture.width * cursorScale);
        int newH = Mathf.RoundToInt(cursorTexture.height * cursorScale);
        cursorTexture = ResizeTexture(cursorTexture, newW, newH);
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
        UpdateSlider(sliderVolume, lastVolume, volumeText);
        UpdateSlider(sliderMusic, lastMusic, musicText);

        if (startMenu.gameObject.activeSelf)
            SetVisibleCursor();

        if (playerInputs.IsStoppingGame())
            TogglePause();
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
                case "Attaquer":
                    kb.buttonText.text = playerInputs.GetAttackKey().ToString();
                    break;
                case "Arc":
                    kb.buttonText.text = playerInputs.GetBowKey().ToString();
                    break;
                case "SwitchSword":
                    kb.buttonText.text = playerInputs.GetSwitchSwordKey().ToString();
                    break;
                case "SwitchBow":
                    kb.buttonText.text = playerInputs.GetSwitchBowKey().ToString();
                    break;
                case "Courir":
                    kb.buttonText.text = playerInputs.GetRunKey().ToString();
                    break;
                case "Pause":
                    kb.buttonText.text = playerInputs.GetPauseKey().ToString();
                    break;
            }
        }
    }



    private void UpdateSliderTextAndColor(float value, TextMeshProUGUI text)
    {
        text.text = $"{Mathf.Round(value * 100)}%";
        text.color = (value <= 0.01f) ? Color.red : baseColorText;
    }

    private void UpdateSlider(Slider slider, float lastValue, TextMeshProUGUI text)
    {
        if (slider.value != lastValue)
        {
            audioSource.volume = slider.value; // ou sliderMusic.value selon contexte
            UpdateSliderTextAndColor(slider.value, text);
            lastValue = slider.value;
        }
    }


    private void TogglePause()
    {
        if (pauseCanvas == null) return;

        isPaused = !isPaused;
        pauseCanvas.gameObject.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        if (isPaused)
            SetVisibleCursor();
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void SetVisibleCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private Texture2D GetTextureFromSprite(Sprite sprite)
    {
        var rect = sprite.textureRect;
        var tex = new Texture2D((int)rect.width, (int)rect.height);
        var pixels = sprite.texture.GetPixels(
            (int)rect.x,
            (int)rect.y,
            (int)rect.width,
            (int)rect.height
        );

        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }


    private Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        RenderTexture.active = rt;

        Graphics.Blit(source, rt);

        Texture2D newTex = new Texture2D(newWidth, newHeight);
        newTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        newTex.Apply();

        RenderTexture.ReleaseTemporary(rt);
        return newTex;
    }


    public void StartGame()
    {
        if (startMenu != null)
            startMenu.gameObject.SetActive(false);

        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        isPaused = false;
        audioSource.PlayOneShot(audioClipsVolume[0]);
    }


    public void StartNewGame()
    {
        Debug.Log("Start new game");
    }

    public void ResumeGame()
    {
        pauseCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
        
        isPaused = false;

        // Masque le curseur et verrouille-le pour le gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void GoToSettings()
    {
        Debug.Log("Go To Settings");
    }

    public void RestartGame()
    {
        Debug.Log("Restart Game");
    }

    public void ShowCredits()
    {
        Debug.Log("Show credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ShowSettingsBloc(GameObject activeBloc)
    {
        blocGame.SetActive(activeBloc == blocGame);
        blocGraphics.SetActive(activeBloc == blocGraphics);
        blocSounds.SetActive(activeBloc == blocSounds);
        blocKeybinds.SetActive(activeBloc == blocKeybinds);
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

    #region Show Any Content
    public void ShowGame() => ShowSettingsBloc(blocGame);
    public void ShowGraphics() => ShowSettingsBloc(blocGraphics);
    public void ShowSounds() => ShowSettingsBloc(blocSounds);
    public void ShowKeybinds() => ShowSettingsBloc(blocKeybinds);
    #endregion


    #region Rebind Any Key
    public void OnRebindButtonClicked(string actionName)
    {
        StartRebind(actionName);
    }
    // public void StartRebindForward(GameObject forwardButton) => StartRebind("Forward", forwardButton);
    // public void StartRebindBackward(GameObject backwardButton) => StartRebind("Forward", backwardButton);
    // public void StartRebindRight(GameObject rightButton) => StartRebind("Forward", rightButton);
    // public void StartRebindLeft(GameObject leftButton) => StartRebind("Forward", leftButton);
    // public void StartRebindRun(GameObject runButton) => StartRebind("Forward", runButton);
    // public void StartRebindJump(GameObject jumpButton) => StartRebind("Forward", jumpButton);

    #endregion


    public void ShowGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        gameOverPanel.gameObject.SetActive(true);
        StartCoroutine(FadeInGameOver());
    }

    private IEnumerator FadeInGameOver()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            gameOverPanel.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
        gameOverPanel.alpha = 1f;
    }

}
