using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Serialized Fields
    [Header("Movement Settings")]
    [SerializeField] private float speedDeplacement = 7f; // Vitesse de déplacement de base
    [SerializeField] private float speedDeplacementRunning = 12f; // Vitesse quand le joueur court
    [SerializeField] private float multipleSpeedDeplacementBackwardAndSide = 0.4f; // On ralentit un peu en marche arrière ou sur le côté
    [SerializeField] private float speedRotation = 100f; // Vitesse de rotation (tournage gauche/droite)

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 5f; // La force appliquée quand le joueur saute

    [Header("Collision")]
    [SerializeField] private LayerMask groundMask; // Layer pour détecter le sol

    [Header("Animations")]
    [SerializeField] private Animator anim; // Animator pour gérer les animations

    #endregion
    #region Private State
    private float rotationVelocity = 0f; // vitesse de rotation actuelle
    private bool isGrounded; // Est-ce que le joueur touche le sol ?
    private bool wasGrounded; // Pour détecter l’atterrissage
    private bool isFalling; // Est-ce que le joueur est en train de tomber ?
    private bool canMove = true; // Est-ce que le joueur peut bouger ?
    private bool canJump = true; // Est-ce que le joueur peut sauter ?
    private bool airAttackRequested = false; // Est-ce qu’on a demandé une attaque en l’air ?
    private bool continueAirAttack = false; // Pour continuer l’animation d’attaque en chute

    private float horizontal; // Input horizontal
    private float vertical;   // Input vertical
    private float currentSpeed; // Vitesse actuelle (variable selon course ou marche)
    private bool moving; // Est-ce que le joueur bouge ?
    private Vector3 inputVector; // Direction de déplacement calculée
    #endregion

    #region Components
    private Transform groundCheck; // Point de référence pour checker le sol
    private Rigidbody rb; // Rigidbody du joueur
    #endregion

    #region Unity Methods
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck"); // On cherche l’objet GroundCheck
        if (groundCheck == null)
            Debug.Log("GroundCheck est null"); // Attention si le GameObject n’existe pas
    }

    void Update()
    {
        UpdateCollisionStatus(); // Vérifie si le joueur est au sol
        GetPlayerInput();        // Récupère les inputs du joueur
        CalculateMovement();     // Calcule la direction et vitesse de déplacement
        HandleAnimations();      // Met à jour les paramètres de l’animator
        HandleRotation();        // Gère la rotation du joueur

        // Détection descente après avoir attaqué en l'air
        if (airAttackRequested && rb.linearVelocity.y < 0)
        {
            continueAirAttack = true;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement(); // Applique le mouvement calculé
    }
    #endregion

    #region Collision
    private void UpdateCollisionStatus()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask); // Sphere check pour savoir si on touche le sol

        // Détecte l’atterrissage
        if (!wasGrounded && isGrounded)
            OnLand();
    }

    private void OnLand()
    {
        if (!airAttackRequested) return; // Si aucune attaque aérienne en cours, on sort

        anim.SetTrigger("AttackAirFall"); // Lance l’animation d’attaque en chute

        airAttackRequested = false;
        continueAirAttack = false;

        canMove = true; // On peut se déplacer de nouveau
    }
    #endregion

    #region Input
    private void GetPlayerInput()
    {
        horizontal = Input.GetAxis("Horizontal"); // A/D ou flèches gauche/droite
        vertical = Input.GetAxis("Vertical");     // Z/S ou flèches haut/bas

        isFalling = rb.linearVelocity.y < -0.1f && !isGrounded; // Détecte la chute

        if (Input.GetKeyDown(KeyCode.Space))
            TryToJump(); // Touche espace -> saut

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isGrounded)
                TryToAttackInAir(); // Clique gauche en l’air -> attaque aérienne
            else
                TryToAttackInGround(); // Clique gauche au sol -> attaque classique
        }
    }
    #endregion

    #region Movement
    private void CalculateMovement()
    {
        if (canMove)
        {
            float adjustedVertical =
                vertical < 0 ? vertical * multipleSpeedDeplacementBackwardAndSide : vertical; // On ralentit si marche arrière

            inputVector = new Vector3(
                horizontal * multipleSpeedDeplacementBackwardAndSide, // Ralentit sur le côté
                0,
                adjustedVertical
            );
        }
        else inputVector = Vector3.zero; // Si on ne peut pas bouger, pas de déplacement

        moving = inputVector.magnitude > 0.1f; // Est-ce qu’on bouge ?

        HandleSprint(); // Vérifie si le joueur court
    }

    private void HandleSprint()
    {
        // On court si on appuie sur Left Shift, qu'on bouge et qu'on avance
        if (Input.GetKey(KeyCode.LeftShift) && moving && vertical > 0)
        {
            anim.SetBool("isRunning", true);
            currentSpeed = speedDeplacementRunning;
        }
        else
        {
            anim.SetBool("isRunning", false);
            currentSpeed = speedDeplacement;
        }
    }

    private void ApplyMovement()
    {
        if (moving)
        {
            Vector3 move = transform.TransformDirection(inputVector)
                            * currentSpeed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + move); // Déplace le joueur
        }
    }

    private void HandleRotation()
    {
        if (!canMove) return;

        float mouseX = Input.GetAxis("Mouse X");
        if (Mathf.Abs(mouseX) > 0.01f)
            rotationVelocity = mouseX * speedRotation;


        
        transform.Rotate(0, rotationVelocity * Time.deltaTime, 0);
        rotationVelocity = Mathf.Lerp(rotationVelocity, 0f, 5f * Time.deltaTime);
    }
    #endregion

    #region Actions
    private void TryToJump()
    {
        airAttackRequested = false;
        continueAirAttack = false;
        anim.ResetTrigger("Attack");
        anim.ResetTrigger("AttackInAir");
        anim.ResetTrigger("AttackAirFall");

        if (!isGrounded || !canJump) return; // On ne peut sauter que si on touche le sol

        anim.SetTrigger("Jump"); // Animation de saut
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Applique la force de saut
    }

    private void TryToAttackInGround()
    {
        anim.SetTrigger("Attack"); // Animation d’attaque au sol
        
    }

    private void TryToAttackInAir()
    {
        anim.SetTrigger("AttackInAir"); // Animation d’attaque aérienne

        canMove = false; // On bloque le mouvement pendant l’attaque
        anim.applyRootMotion = false; // Désactive le root motion pour contrôler le mouvement via script
        airAttackRequested = true; // On marque qu’une attaque aérienne est en cours
    }
    #endregion

    #region Animations
    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", inputVector.x, 0.1f, Time.deltaTime); // Param pour blend tree
        anim.SetFloat("zVelocity", inputVector.z, 0.1f, Time.deltaTime); // Param pour blend tree
        anim.SetBool("isFalling", isFalling); // Bool pour animation de chute
        anim.SetBool("isMoving", moving); // Bool pour animation de marche
    }
    #endregion

    #region Public Methods
    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }

    public void ResetContinueAttackInAir() {
        continueAirAttack = false;
        anim.applyRootMotion = true; // On remet le root motion après l’attaque aérienne
    } 

    public void PrepareAirAttackFall() => airAttackRequested = true; // Prépare une attaque en chute
    #endregion

    

    
}
