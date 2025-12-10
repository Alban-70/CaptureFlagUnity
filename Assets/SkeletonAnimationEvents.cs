using UnityEngine;

public class SkeletonAnimationEvents : MonoBehaviour
{
    [SerializeField] private SkeletonLogic skeleton;


    private void DisableMovement() => skeleton.EnableMovement(false);
    private void EnableMovement() => skeleton.EnableMovement(true);
    private void DealDamage() {
        Debug.Log("dealdamage");
        skeleton.SetDamageToPlayer();
    }
}
