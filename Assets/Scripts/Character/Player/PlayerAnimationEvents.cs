using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerMovement player;
    private PlayerCombat playerCombat;
    private WeaponSwitcher weaponSwitcher;


    void Awake()
    {
        player = transform.parent.parent.GetComponentInParent<PlayerMovement>();
        playerCombat = transform.parent.parent.GetComponentInParent<PlayerCombat>();
        weaponSwitcher = transform.parent.parent.GetComponent<WeaponSwitcher>();
        SwitchSwordToHand();
        SwitchBowToBack();
    }

    private void DisableMovementAndJump() 
        => player.EnableMovementAndJump(false);

    private void EnableMovementAndJump() 
        => player.EnableMovementAndJump(true);

    private void BeginAirAttackFall() 
        => playerCombat.PrepareAirAttackFall();

    private void ResetContinueAttackInAirAnim() 
        => playerCombat.ResetContinueAttackInAir();

    private void SwitchSwordToHand() 
        => weaponSwitcher.SwitchSwordToHand();

    private void SwitchSwordToBack()
        => weaponSwitcher.SwitchSwordToBack();

    private void SwitchBowToHand()
        => weaponSwitcher.SwitchBowToHand();

    private void SwitchBowToBack()
        => weaponSwitcher.SwitchBowToBack();

    private void TakeArrowFromBack()
    {
        
    }

    private void LaunchArrow()
    {
        playerCombat.LaunchArrow();
    }
}
