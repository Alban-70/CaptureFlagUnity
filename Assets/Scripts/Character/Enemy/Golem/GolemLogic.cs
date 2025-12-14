using UnityEngine;

public class GolemLogic : EnemyLogic
{
    protected override void Awake()
    {
        moveSpeed = 0.8f;
        attackDistance = 3f;
        attackDamage = 20f;
        base.Awake();
    }
}
