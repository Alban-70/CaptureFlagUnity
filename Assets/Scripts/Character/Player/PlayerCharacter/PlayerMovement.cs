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

    [Header("Components")]
    [SerializeField] private Animator anim; // Animator pour gérer les animations
    private Transform groundCheck; // Point de référence pour checker le sol
    private Rigidbody rb; // Rigidbody du joueur

    #endregion
    #region Private State
    private bool isGrounded; // Est-ce que le joueur touche le sol ?
    private bool wasGrounded; // Pour détecter l’atterrissage
    private bool isFalling; // Est-ce que le joueur est en train de tomber ?
    private bool canMove = true; // Est-ce que le joueur peut bouger ?
    private bool canJump = true; // Est-ce que le joueur peut sauter ?

    private float horizontal; // Input horizontal
    private float vertical;   // Input vertical
    private Vector3 inputVector; // Direction de déplacement calculée
    private float currentSpeed; // Vitesse actuelle (variable selon course ou marche)
    private bool moving; // Est-ce que le joueur bouge ?
    private float rotationVelocity = 0f; // vitesse de rotation actuelle
    private bool isHoldingBow = false;

    [SerializeField] private PlayerInputs inputs;
    #endregion

    [HideInInspector] public bool airAttackRequested = false;

    #region Unity Methods
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck"); // On cherche l’objet GroundCheck
    }

    void Update()
    {
        ReadInputs();
        UpdateCollisionStatus(); // Vérifie si le joueur est au sol
        // GetPlayerInput();        // Récupère les inputs du joueur
        CalculateMovement();     // Calcule la direction et vitesse de déplacement
        HandleRotation();        // Gère la rotation du joueur
        HandleAnimations();      // Met à jour les paramètres de l’animator

        if (inputs.IsJumpPressed())
            TryToJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement(); // Applique le mouvement calculé
    }
    #endregion

    private void ReadInputs()
    {
        horizontal = inputs.GetHorizontal();
        vertical = inputs.GetVertical();

        isFalling = rb.linearVelocity.y < -0.1f && !isGrounded;
    }

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
        if (airAttackRequested)
        {
            anim.SetTrigger("AttackAirFall"); // Lance l’animation d’attaque en chute
            airAttackRequested = false;
        }
        canMove = true; // On peut se déplacer de nouveau
 
    }
    #endregion

    #region Input - Main
    public void SetBowHold(bool value)
    {
        isHoldingBow = value;
    }
    #endregion

    #region Movement
    private void CalculateMovement()
    {
        if (!canMove) 
        { 
            inputVector = Vector3.zero; 
            moving = false; 
            return; 
        }
        // if (currentWeapon == WeaponType.Bow && isHoldingBow)
        //     return; 
        
        float adjustedVertical =
                vertical < 0 ? vertical * multipleSpeedDeplacementBackwardAndSide : vertical; // On ralentit si marche arrière
    
        float sideMultiplier = isHoldingBow ? multipleSpeedDeplacementBackwardAndSide : 1f;
        
        inputVector = new Vector3(
                horizontal * sideMultiplier, // Ralentit sur le côté
                0,
                adjustedVertical * sideMultiplier);

        moving = inputVector.magnitude > 0.1f; // Est-ce qu’on bouge ?
        HandleSprint(); // Vérifie si le joueur court
    }

    private void HandleSprint()
    {
        if (inputs.IsJumpPressed() || !moving)
        {
            currentSpeed = speedDeplacement;
            anim.SetBool("isRunning", false);
            return;
        }
        // On court si on appuie sur Left Shift, qu'on bouge et qu'on avance
        if (inputs.IsRunPressed() && vertical > 0)
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
        if (!moving) return;
        Vector3 move = transform.TransformDirection(inputVector) * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move); // Déplace le joueur

    }

    private void HandleRotation()
    {
        if (!canMove) return;

        float mouseX = inputs.GetMouseX();
        if (Mathf.Abs(mouseX) > 0.01f)
            rotationVelocity = mouseX * speedRotation;

        transform.Rotate(0, rotationVelocity * Time.deltaTime, 0);
        rotationVelocity = Mathf.Lerp(rotationVelocity, 0f, 5f * Time.deltaTime);
    }
    #endregion

    #region Actions
    private void TryToJump()
    {
        if (!isGrounded || !canJump) return; // On ne peut sauter que si on touche le sol

        anim.ResetTrigger("SwordAttack");
        anim.ResetTrigger("SwordAttackInAir");
        anim.ResetTrigger("AttackAirFall");

        anim.SetTrigger("Jump"); // Animation de saut
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Applique la force de saut
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
    #endregion

    
    #region Getters

    public bool IsGrounded() { return isGrounded; }

    #endregion
    
}
