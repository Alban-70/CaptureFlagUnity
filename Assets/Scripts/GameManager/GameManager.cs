using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{

    [Header("Manager scripts")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private PauseManager pauseManager; 
    [SerializeField] private KeyBindManager keyBindManager; 
    [SerializeField] private UI_Manager uI_Manager; 

    

    [Header("Timeline & Camera Cinemachine")]
    [SerializeField] private PlayableDirector introTimeline;
    [SerializeField] private Camera cinemachineCamera;

    [Header("UI Elements")]
    [SerializeField] private Slider sliderVolume; 
    [SerializeField] private Slider sliderMusic; 
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI volumeText;
    [SerializeField] private Image coinImage;

    [SerializeField] private Loader loader;


    [Header("Basic color")]
    [SerializeField] private Color baseColorText;

    public bool getCoin = false;

    void Awake()
    {
        coinImage.gameObject.SetActive(false);
        uI_Manager.ShowStartMenu();
    }


    void Update()
    {
        
        UpdateSlider(sliderVolume, volumeText);
        UpdateSlider(sliderMusic, musicText);

        if (getCoin)
            coinImage.gameObject.SetActive(true);
        else 
            coinImage.gameObject.SetActive(false);
    }

    private void UpdateSliderTextAndColor(float value, TextMeshProUGUI text)
    {
        text.text = $"{Mathf.Round(value * 100)}%";
        text.color = (value <= 0.01f) ? Color.red : baseColorText;
    }

    private void UpdateSlider(Slider slider, TextMeshProUGUI text)
    {
        audioManager.SetVolume(slider.value);
        UpdateSliderTextAndColor(slider.value, text);

    }


    public void StartGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        uI_Manager.HideStartMenu();
        Time.timeScale = 1f;

        if (introTimeline != null)
        {
            uI_Manager.HideUI_ElementsInGame();
            introTimeline.Play();
        }
        else
        {
            // fallback si pas de Timeline
            OnIntroCinematicFinished();
        }
    }

    public void OnIntroCinematicFinished()
    {
        cinemachineCamera.gameObject.SetActive(false);
        uI_Manager.ShowUI_ElementsInGame();

        audioManager.PlayDebutJeu();
    }

    public void ShowWinning()
    {
        Time.timeScale = 0f;
        uI_Manager.ShowGameWinPanel();
    }

    public void StartNewGame()
    {
        Debug.Log("Start new game");
    }

    public void GoToSettings()
    {
        uI_Manager.ShowSettings();
    }
    public void ShowCredits()
    {
        uI_Manager.ShowCredits();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0f;
        uI_Manager.ShowGameOverPanel();
    }

}
