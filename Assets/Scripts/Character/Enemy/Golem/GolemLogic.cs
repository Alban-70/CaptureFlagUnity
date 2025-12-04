using UnityEngine;
using UnityEngine.AI;

public class GolemLogic : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private PlayerHealth playerHealth;
    #endregion

    #region Private Variables
    private float moveSpeed = 0.8f;
    private float attackDistance = 2.5f;
    private float attackDamage = 20f;
    private float distanceToPlayer;
    private bool canMove = true;
    private bool isAttacking;
    #endregion

    #region Unity Methods
    void Awake()
    {
        if (targetPlayer == null)
            Debug.LogError("Player is not assigned in EnemyLogic !");
        
        navMeshAgent.speed = moveSpeed;
    }

    void Update()
    {
        CalculateMovementDirection();
        UpdateAnimations();
    }

    void LateUpdate()
    {
        UpdateAttackState();
    }
    #endregion

    #region Attack State
    private void UpdateAttackState()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        isAttacking = state.IsTag("Attack");
    }
    #endregion

    #region Movement Logic
    private void CalculateMovementDirection()
    {
        if (!canMove)
            return;

        distanceToPlayer = Vector3.Distance(navMeshAgent.transform.position, targetPlayer.position);
        if (distanceToPlayer < attackDistance || isAttacking)
        {
            navMeshAgent.isStopped = true;
            if (!isAttacking)
            {
                anim.SetTrigger("Attack");
                
            }
        } 
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = targetPlayer.position;
        }
    }
    #endregion

    #region 
    public void DealDamage()
    {
        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        if (distance <= attackDistance)
        {
            playerHealth = targetPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.ApplyDamage(attackDamage);
            }
        }
    }
    #endregion

    #region Animation logic
    private void UpdateAnimations()
    {
        // Récupère la vélocité réelle du NavMeshAgent
        Vector3 worldVel = navMeshAgent.velocity;

        // Convertit en direction locale (nécessaire pour blend tree 2D)
        Vector3 localVel = transform.InverseTransformDirection(worldVel);

        bool moving = worldVel.magnitude > 0.05f;

        anim.SetFloat("xVelocity", localVel.x, 0.1f, Time.deltaTime);
        anim.SetFloat("zVelocity", localVel.z, 0.1f, Time.deltaTime);

        anim.SetBool("isMoving", moving);
    }

    public void EnableMovement(bool enable) => canMove = enable;

    #endregion
}
