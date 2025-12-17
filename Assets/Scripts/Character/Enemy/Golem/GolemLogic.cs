using UnityEngine;

public class GolemLogic : EnemyLogic
{
    protected override void Awake()
    {
        moveSpeed = 7f;
        attackDistance = 3f;
        attackDamage = 20f;
        base.Awake();
    }
}
