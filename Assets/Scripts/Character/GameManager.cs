using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image imageFill;

    public void SetHealth(float currentHealth) => imageFill.fillAmount = currentHealth;
}
