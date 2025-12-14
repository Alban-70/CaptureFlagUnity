using UnityEngine;

public class GolemLogic : EnemyLogic
{
    protected override void Awake()
    {
        moveSpeed = 0.8f;
        attackDistance = 2.5f;
        attackDamage = 20f;
        base.Awake();
    }
}
