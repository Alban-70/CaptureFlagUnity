using UnityEngine;
using UnityEngine.AI;

public class GolemLogic : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private Transform targetPlayer; // Référence au joueur pour le suivi
    [SerializeField] private Animator anim; // Animator pour gérer les animations
    [SerializeField] private NavMeshAgent navMeshAgent; // NavMeshAgent pour le déplacement AI
    [SerializeField] private HealthSystem healthSystem; // Référence à la santé du joueur
    #endregion

    #region Private Variables
    private float moveSpeed = 0.8f; // Vitesse de déplacement du Golem
    private float attackDistance = 2.5f; // Distance à laquelle le Golem attaque
    private float attackDamage = 20f; // Dégâts infligés au joueur
    private float distanceToPlayer; // Distance actuelle entre le Golem et le joueur
    private bool canMove = true; // Permet de bloquer le déplacement si nécessaire
    private bool isAttacking; // Indique si le Golem est en train d'attaquer
    #endregion

    #region Unity Methods
    /// <summary>
    /// Initialisation du Golem : vérifie le joueur et configure la vitesse du NavMeshAgent.
    /// </summary>
    void Awake()
    {
        if (targetPlayer == null)
            Debug.LogError("Player is not assigned in EnemyLogic !");
        
        navMeshAgent.speed = moveSpeed;
    }

    /// <summary>
    /// Méthode appelée à chaque frame pour gérer le mouvement et les animations.
    /// </summary>
    void Update()
    {
        CalculateMovementDirection();
        UpdateAnimations();
    }

    /// <summary>
    /// Méthode appelée après Update pour mettre à jour l'état d'attaque basé sur l'animation.
    /// </summary>
    void LateUpdate()
    {
        UpdateAttackState();    
    }
    #endregion

    #region Attack State
    /// <summary>
    /// Met à jour la variable isAttacking en fonction de l'animation actuelle.
    /// </summary>
    private void UpdateAttackState()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        isAttacking = state.IsTag("Attack");
    }
    #endregion

    #region Movement Logic
    /// <summary>
    /// Calcule la direction vers le joueur et décide si le Golem doit se déplacer ou attaquer.
    /// </summary>
    private void CalculateMovementDirection()
    {
        if (!canMove) 
            return;

        if (targetPlayer == null) return;

        HealthSystem playerHealth = targetPlayer.GetComponent<HealthSystem>();
        if (playerHealth != null && playerHealth.IsDead())
        {
            targetPlayer = null;       // on enlève la cible
            navMeshAgent.isStopped = true;
            anim.SetBool("isMoving", false);
            return;
        }
        
        distanceToPlayer = Vector3.Distance(navMeshAgent.transform.position, targetPlayer.position);

        if (distanceToPlayer < attackDistance || isAttacking)
        {
            navMeshAgent.isStopped = true;
            if (!isAttacking)
                anim.SetTrigger("Attack");
        } 
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = targetPlayer.position;
        }
    }
    #endregion

    #region Damage Logic
    /// <summary>
    /// Inflige des dégâts au joueur si celui-ci est à portée.
    /// </summary>
    public void SetDamageToPlayer()
    {
        if (targetPlayer == null) return;

        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        if (distance <= attackDistance)
        {
            if (healthSystem != null)
                healthSystem.ApplyDamage(attackDamage);
        }
    }
    #endregion

    #region Animation Logic
    /// <summary>
    /// Met à jour les paramètres de l'Animator en fonction de la vélocité du NavMeshAgent.
    /// </summary>
    private void UpdateAnimations()
    {
        Vector3 worldVel = navMeshAgent.velocity;
        Vector3 localVel = transform.InverseTransformDirection(worldVel);
        bool moving = worldVel.magnitude > 0.05f;

        anim.SetFloat("xVelocity", localVel.x, 0.1f, Time.deltaTime);
        anim.SetFloat("zVelocity", localVel.z, 0.1f, Time.deltaTime);
        anim.SetBool("isMoving", moving);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Active ou désactive le mouvement du Golem.
    /// </summary>
    /// <param name="enable">true pour permettre le mouvement, false pour le bloquer.</param>
    public void EnableMovement(bool enable) => canMove = enable;
    #endregion
}
