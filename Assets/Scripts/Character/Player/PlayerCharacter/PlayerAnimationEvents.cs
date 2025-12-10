using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private WeaponSwitcher weaponSwitcher;
    [SerializeField] private ArrowScript arrowScript;


    void Awake()
    {
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
    
    private void SetDamageToEnemyWithSword()
        => playerCombat.SetDamageToEnemyWithSword();

    private void ResetJumpTrigger() 
        => player.ResetJumpTrigger();

    private void ResetSwordHitList()
        => playerCombat.ResetSwordHitList();

    private void LaunchArrow()
    {
        playerCombat.LaunchArrow();
    }
}
