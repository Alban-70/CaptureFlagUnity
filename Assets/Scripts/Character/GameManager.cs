using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image imageFill;
    [SerializeField] private TextMeshProUGUI text;

    private float targetFill;
    private float speed = 5f;

    public void SetMaxHealth()
    {
        targetFill = 1f;                // la vie max = barre pleine
        imageFill.fillAmount = 1f;
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        targetFill = currentHealth / maxHealth;
    }

    public void SetText(float appendText)
    {
        text.SetText(appendText.ToString());
        if (appendText <= 20)
            SetColorText(Color.red);
        else
            SetColorText(Color.white);
    }

    private void SetColorText(Color color)
    {
        text.color = color;
    }

    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(
            imageFill.fillAmount,
            targetFill,
            Time.deltaTime * speed
        );
    }
}
