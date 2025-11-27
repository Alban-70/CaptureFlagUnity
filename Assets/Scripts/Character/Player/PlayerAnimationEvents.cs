using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{

    private PlayerMovement player;


    void Awake()
    {
        player = transform.parent.parent.GetComponentInParent<PlayerMovement>();
    }

    private void DisableMovementAndJump() => player.EnableMovementAndJump(false);
    private void EnableMovementAndJump() => player.EnableMovementAndJump(true);
}
