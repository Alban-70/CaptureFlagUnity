using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject swordHand;   // Épée dans la main
    [SerializeField] private GameObject swordBack;   // Épée dans le dos
    [SerializeField] private GameObject bowBack;     // Arc dans le dos
    [SerializeField] private GameObject bowHand;     // Arc dans la main

    /// <summary>
    /// Déplace l'épée du dos vers la main.
    /// </summary>
    public void SwitchSwordToHand()
    {
        swordBack.SetActive(false);
        swordHand.SetActive(true);
    }

    /// <summary>
    /// Replace l'épée de la main vers le dos.
    /// </summary>
    public void SwitchSwordToBack()
    {
        swordHand.SetActive(false);
        swordBack.SetActive(true);
    }

    /// <summary>
    /// Déplace l'arc du dos vers la main.
    /// </summary>
    public void SwitchBowToHand()
    {
        bowBack.SetActive(false);
        bowHand.SetActive(true);
    }

    /// <summary>
    /// Replace l'arc de la main vers le dos.
    /// </summary>
    public void SwitchBowToBack()
    {
        bowHand.SetActive(false);
        bowBack.SetActive(true);
    }
}
