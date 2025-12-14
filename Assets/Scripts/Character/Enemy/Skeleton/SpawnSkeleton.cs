using UnityEngine;

public class SpawnSkeleton : EnemyLogic
{
    protected override void Awake()
    {
        moveSpeed = 6f;
        attackDistance = 1f;
        attackDamage = 5f;
        base.Awake();
    }
}
