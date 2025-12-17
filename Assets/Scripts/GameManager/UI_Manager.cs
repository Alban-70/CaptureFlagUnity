using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;

    [Header("UI References")]
    [SerializeField] private CanvasGroup gameOverPanel;
    [SerializeField] private CanvasGroup winningImage;
    [SerializeField] private Canvas startMenu;
    [SerializeField] private Image overlay;
    [SerializeField] private Image settingsCanvas;
    [SerializeField] private Image creditsCanvas;

    [Header("Elements to hide during cinematic")]
    [SerializeField] private GameObject miniMap;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject arrowContainer;
    [SerializeField] private GameObject questsContainer;

    public TextMeshProUGUI textQuest;
    public Image targetQuestImage;
    public Sprite questsDone;

    [Header("Differents Blocs de settings")]
    [SerializeField] private GameObject blocGame;
    [SerializeField] private GameObject blocGraphics;
    [SerializeField] private GameObject blocSounds;
    [SerializeField] private GameObject blocKeybinds;

    [Header("Cursor")]
    [SerializeField] private Sprite customCursor;


    private Texture2D cursorTexture;
    private float cursorScale = 0.5f;
    private float fadeDuration = 5f;
    private bool showCrosshair = false;
    private bool isInCinematic = true;



    void Awake()
    {
        if (winningImage != null)
            winningImage.gameObject.SetActive(false);
        if (startMenu != null)
            startMenu.gameObject.SetActive(true);

        cursorTexture = GetTextureFromSprite(customCursor);
        int newW = Mathf.RoundToInt(cursorTexture.width * cursorScale);
        int newH = Mathf.RoundToInt(cursorTexture.height * cursorScale);
        cursorTexture = ResizeTexture(cursorTexture, newW, newH);
    }

    void Update()
    {
        if (startMenu.gameObject.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }

        if (!isInCinematic)
        {
            crosshair.gameObject.SetActive(
                playerCombat.currentWeapon == PlayerCombat.WeaponType.Bow
            );
        }
        
    }


    public void ShowUI_ElementsInGame()
    {
        isInCinematic = false;
        miniMap.SetActive(true);
        crosshair.SetActive(true);
        healthBar.SetActive(true);
        arrowContainer.SetActive(true);
        questsContainer.SetActive(true);
    }

    public void HideUI_ElementsInGame()
    {
        isInCinematic = true;
        miniMap.SetActive(false);
        crosshair.SetActive(false);
        healthBar.SetActive(false);
        arrowContainer.SetActive(false);
        questsContainer.SetActive(false);
    }

    public void TextCaptureZone()
    {
        textQuest.text = "Capturer la zone";
    }

    public void ChangeQuestImage()
    {
        textQuest.text = "Quêtes terminées";
        textQuest.color = Color.green;
        targetQuestImage.sprite = questsDone;
    }

    public void ShowStartMenu()
    {
        Time.timeScale = 0f;
        startMenu.gameObject.SetActive(true);
        SetVisibleCursor();
    }

    public void HideStartMenu()
    {
        startMenu.gameObject.SetActive(false);
    }

    public void ShowOverlay()
    {
        overlay.gameObject.SetActive(true);
    }

    public void HideOverlay()
    {
        overlay.gameObject.SetActive(false);
    }

    public void ShowSettings()
    {
        HideOverlay();
        settingsCanvas.gameObject.SetActive(true);
    }

    public void HideSettings()
    {
        settingsCanvas.gameObject.SetActive(false);
        ShowOverlay();
    }

    public void ShowCredits()
    {
        creditsCanvas.gameObject.SetActive(true);
    }

    public void HideCredits()
    {
        creditsCanvas.gameObject.SetActive(false);
        ShowOverlay();
    }

    public void ShowGameOverPanel()
    {
        SetVisibleCursor();
        gameOverPanel.gameObject.SetActive(true);
        StartCoroutine(FadeInCanvas(gameOverPanel));
    }

    public void ShowGameWinPanel()
    {
        SetVisibleCursor();
        winningImage.gameObject.SetActive(true);
        StartCoroutine(FadeInCanvas(winningImage));
    }

    private void ShowSettingsBloc(GameObject activeBloc)
    {
        blocGame.SetActive(activeBloc == blocGame);
        blocGraphics.SetActive(activeBloc == blocGraphics);
        blocSounds.SetActive(activeBloc == blocSounds);
        blocKeybinds.SetActive(activeBloc == blocKeybinds);
    }

    #region Show Any Content
    public void ShowGame() => ShowSettingsBloc(blocGame);
    public void ShowGraphics() => ShowSettingsBloc(blocGraphics);
    public void ShowSounds() => ShowSettingsBloc(blocSounds);
    public void ShowKeybinds() => ShowSettingsBloc(blocKeybinds);
    #endregion

    private IEnumerator FadeInCanvas(CanvasGroup canvasGroup)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
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
}
