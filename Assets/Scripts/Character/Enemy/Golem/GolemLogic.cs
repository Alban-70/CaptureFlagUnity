using UnityEngine;
using UnityEngine.AI;

public class GolemLogic : MonoBehaviour
{
    #region Serialized Fields
    [Header("References")]
    [SerializeField] private Transform targetPlayer; // Référence au joueur pour le suivi
    [SerializeField] private Animator anim; // Animator pour gérer les animations
    [SerializeField] private NavMeshAgent navMeshAgent; // NavMeshAgent pour le déplacement AI
    [SerializeField] private PlayerHealth playerHealth; // Référence à la santé du joueur
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
    void Awake()
    {
        // Vérifie si le joueur est assigné
        if (targetPlayer == null)
            Debug.LogError("Player is not assigned in EnemyLogic !");
        
        // Initialise la vitesse du NavMeshAgent
        navMeshAgent.speed = moveSpeed;
    }

    void Update()
    {
        // Calcul la direction et la logique de déplacement
        CalculateMovementDirection();

        // Met à jour les paramètres de l'Animator pour les animations de mouvement
        UpdateAnimations();
    }

    void LateUpdate()
    {
        // Met à jour l'état d'attaque basé sur l'animation actuelle
        UpdateAttackState();    
    }
    #endregion

    #region Attack State
    private void UpdateAttackState()
    {
        // Vérifie si l'animation actuelle est une attaque
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        isAttacking = state.IsTag("Attack");
    }
    #endregion

    #region Movement Logic
    private void CalculateMovementDirection()
    {
        if (!canMove) // Si le Golem ne peut pas bouger, quitte la fonction
            return;

        // Calcule la distance entre le Golem et le joueur
        distanceToPlayer = Vector3.Distance(navMeshAgent.transform.position, targetPlayer.position);
        if (distanceToPlayer < attackDistance || isAttacking)
        {
             // Si proche du joueur ou en train d'attaquer, arrêter le déplacement
            navMeshAgent.isStopped = true;
            if (!isAttacking)
                anim.SetTrigger("Attack"); // Déclenche l'animation d'attaque
        } 
        else
        {
            // Si trop loin, continuer à se déplacer vers le joueur
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = targetPlayer.position;
        }
    }
    #endregion

    #region 
    public void DealDamage()
    {
        // Vérifie si le joueur est encore dans la portée d'attaque
        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        if (distance <= attackDistance)
        {
            if (playerHealth != null)
            {
                // Applique les dégâts au joueur
                playerHealth.ApplyDamage(attackDamage);
            }
        }
    }
    #endregion

    #region Animation Logic
    private void UpdateAnimations()
    {
        // Récupère la vélocité actuelle du NavMeshAgent
        Vector3 worldVel = navMeshAgent.velocity;

        // Convertit la vélocité du monde en local (utile pour blend tree 2D)
        Vector3 localVel = transform.InverseTransformDirection(worldVel);

        // Détermine si le Golem se déplace réellement
        bool moving = worldVel.magnitude > 0.05f;

        // Met à jour les paramètres de l'Animator
        anim.SetFloat("xVelocity", localVel.x, 0.1f, Time.deltaTime);
        anim.SetFloat("zVelocity", localVel.z, 0.1f, Time.deltaTime);
        anim.SetBool("isMoving", moving);
    }
    #endregion

    #region Public Methods
    // Permet d'activer ou désactiver le mouvement depuis d'autres scripts
    public void EnableMovement(bool enable) => canMove = enable;

    #endregion
}
