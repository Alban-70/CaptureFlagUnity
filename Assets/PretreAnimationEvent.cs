using UnityEngine;

public class PretreAnimationEvent : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    private void Win() => healthSystem.ShowGameWinning();
}
