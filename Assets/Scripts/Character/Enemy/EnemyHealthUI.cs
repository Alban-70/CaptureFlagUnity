using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Image imageFill;
    // [SerializeField] private TextMeshProUGUI text;

    private float targetFill;
    private float speed = 5f;

    public void SetMaxHealth(float maxHealth)
    {
        targetFill = 1f;
        imageFill.fillAmount = 1f;
        // text.text = maxHealth.ToString();
        // text.color = Color.white;
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        targetFill = currentHealth / maxHealth;
        // text.text = currentHealth.ToString();
        // text.color = currentHealth <= 20 ? Color.red : Color.white;
    }

    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, targetFill, Time.deltaTime * speed);
    }
}
