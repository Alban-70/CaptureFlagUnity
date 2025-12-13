using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthSystem target;        // Ce dont on affiche la vie
    [SerializeField] private Image imageFill;
    [SerializeField] private TextMeshProUGUI text;

    private float targetFill;
    private float speed = 5f;

    void Update()
    {
        if (target == null) return;
        
        targetFill = target.currentHealth / target.maxHealth;
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, targetFill, Time.deltaTime * speed);

        if (text != null)
        {
            text.text = Mathf.CeilToInt(target.currentHealth).ToString();
            text.color = (target.currentHealth <= 20f) ? Color.red : Color.white;
        }
    }
}
