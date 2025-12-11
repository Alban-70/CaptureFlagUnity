using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private PlayerInputs playerInputs;
    [SerializeField] private Canvas startMenu;
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private CanvasGroup gameOverPanel;
    [SerializeField] private Sprite customCursor;
    [SerializeField] private AudioClip[] audioClips;
    
    
    private AudioSource beginGame;
    private Texture2D cursorTexture;
    private float cursorScale = 0.5f;
    private bool isPaused = false;
    private bool isGameOver = false;
    private float fadeDuration = 5f;

    void Awake()
    {

        beginGame = GetComponent<AudioSource>();

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
        if (startMenu.gameObject.activeSelf)
            SetVisibleCursor();

        if (playerInputs.IsStoppingGame())
            TogglePause();

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
        beginGame.PlayOneShot(audioClips[0]);
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
