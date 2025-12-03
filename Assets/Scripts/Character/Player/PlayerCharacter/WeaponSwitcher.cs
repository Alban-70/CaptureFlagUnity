using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] GameObject swordHand;
    [SerializeField] GameObject swordBack;
    [SerializeField] GameObject bowBack;
    [SerializeField] GameObject bowHand;
    [SerializeField] GameObject bowString;


    public void SwitchSwordToHand()
    {
        swordBack.SetActive(false);
        swordHand.SetActive(true);
    }

    public void SwitchSwordToBack()
    {
        swordHand.SetActive(false);
        swordBack.SetActive(true);
    }

    public void SwitchBowToHand()
    {
        bowBack.SetActive(false);
        bowHand.SetActive(true);
    }

    public void SwitchBowToBack()
    {
        bowHand.SetActive(false);
        bowBack.SetActive(true);
    }

    public void ShowBowString() 
        => bowString.SetActive(true);
    public void HideBowString() 
        => bowString.SetActive(false);
}
