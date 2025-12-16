using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private Transform targetPlayer; // Référence au joueur pour le suivi
    [SerializeField] private Animator anim; // Animator pour gérer les animations
    [SerializeField] private NavMeshAgent navMeshAgent; // NavMeshAgent pour le déplacement AI
    #endregion

    #region Private Variables
    private HealthSystem playerHealth; // Référence à la santé du joueur
    protected float moveSpeed = 0.8f; // Vitesse de déplacement du Golem
    protected float attackDistance = 2.5f; // Distance à laquelle le Golem attaque
    protected float attackDamage = 20f; // Dégâts infligés au joueur
    protected float distanceToPlayer; // Distance actuelle entre le Golem et le joueur
    protected bool canMove = true; // Permet de bloquer le déplacement si nécessaire
    protected bool isAttacking; // Indique si le Golem est en train d'attaquer
    #endregion


    protected virtual void Awake()
    {
        navMeshAgent.speed = moveSpeed;
    }


    protected virtual void Update()
    {
        if (targetPlayer == null) return;

        if (playerHealth == null) 
            playerHealth = targetPlayer.GetComponent<HealthSystem>();

        CalculateMovementDirection();
        UpdateAnimations();
    }

    protected virtual void LateUpdate()
    {
        UpdateAttackState();
    }

    protected void UpdateAttackState()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        isAttacking = state.IsTag("Attack");
    }

    protected virtual void CalculateMovementDirection()
    {
        if (!canMove || targetPlayer == null) return;

        if (playerHealth != null && playerHealth.IsDead())
        {
            navMeshAgent.isStopped = true;
            anim.SetBool("isMoving", false);
            return;
        }

        distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

        if (distanceToPlayer < attackDistance || isAttacking)
        {
            navMeshAgent.isStopped = true;
            if (!isAttacking) anim.SetTrigger("Attack");
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = targetPlayer.position;
        }
    }

    protected virtual void UpdateAnimations()
    {
        Vector3 worldVel = navMeshAgent.velocity;
        Vector3 localVel = transform.InverseTransformDirection(worldVel);
        bool moving = worldVel.magnitude > 0.05f;

        anim.SetFloat("xVelocity", localVel.x, 0.1f, Time.deltaTime);
        anim.SetFloat("zVelocity", localVel.z, 0.1f, Time.deltaTime);
        anim.SetBool("isMoving", moving);
    }

    public virtual void SetDamageToPlayer()
    {
        if (targetPlayer == null) return;

        

        float distance = Vector3.Distance(transform.position, targetPlayer.position);
        if (distance <= attackDistance && playerHealth != null)
            playerHealth.ApplyDamage(attackDamage);
    }

    public void EnableMovement(bool enable)
    {
        canMove = enable;
        navMeshAgent.isStopped = !enable;
        if (!enable)
        {
            navMeshAgent.velocity = Vector3.zero;
            anim.SetBool("isMoving", false);
        }
    }

    public void SetTarget(Transform player) => targetPlayer = player;


}
