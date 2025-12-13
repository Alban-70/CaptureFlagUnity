using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Loader : MonoBehaviour
{
    [Header("UI")]
    public GameObject loadingPanel;
    public Slider progressBar;

    [Header("Settings")]
    public int steps = 10;
    public float minLoadTime = 2f;

    public void StartLoading(string sceneName)
    {
        loadingPanel.SetActive(true);
        StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float timer = 0f;
        int currentStep = 0;

        while (!operation.isDone)
        {
            timer += Time.unscaledDeltaTime;

            float fakeProgress = Mathf.Clamp01(timer / minLoadTime);
            int targetStep = Mathf.FloorToInt(fakeProgress * steps);

            if (targetStep > currentStep)
            {
                currentStep = targetStep;
                progressBar.value = (float)currentStep / steps;
            }

            if (fakeProgress >= 1f && operation.progress >= 0.9f)
            {
                progressBar.value = 1f;
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
