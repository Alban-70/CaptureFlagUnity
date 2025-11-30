using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerMovement player;
    private WeaponSwitcher weaponSwitcher;


    void Awake()
    {
        player = transform.parent.parent.GetComponentInParent<PlayerMovement>();
        weaponSwitcher = transform.parent.parent.GetComponent<WeaponSwitcher>();
        SwitchSwordToHand();
        SwitchBowToBack();
    }

    private void DisableMovementAndJump() 
        => player.EnableMovementAndJump(false);

    private void EnableMovementAndJump() 
        => player.EnableMovementAndJump(true);

    private void BeginAirAttackFall() 
        => player.PrepareAirAttackFall();

    private void ResetContinueAttackInAirAnim() 
        => player.ResetContinueAttackInAir();

    private void SwitchSwordToHand() 
        => weaponSwitcher.SwitchSwordToHand();

    private void SwitchSwordToBack()
        => weaponSwitcher.SwitchSwordToBack();

    private void SwitchBowToHand()
        => weaponSwitcher.SwitchBowToHand();

    private void SwitchBowToBack()
        => weaponSwitcher.SwitchBowToBack();
}
