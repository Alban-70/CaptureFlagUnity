using UnityEngine;

public class GolemAnimationEvents : MonoBehaviour
{
    [SerializeField] private GolemLogic golem;


    private void DisableMovement() => golem.EnableMovement(false);
    private void EnableMovement() => golem.EnableMovement(true);

    private void DealDamage() => golem.DealDamage();
}
