using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private Canvas pauseCanvas;

    private bool isPaused = false;


    void Awake()
    {
        if (pauseCanvas != null)
            pauseCanvas.gameObject.SetActive(false);
    }

    public void TogglePause()
    {
        if (pauseCanvas == null) return;

        isPaused = !isPaused;
        pauseCanvas.gameObject.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
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
}
