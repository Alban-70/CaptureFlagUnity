using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Serialized Fields
    private float speedDeplacement = 7f; // Vitesse de déplacement de base
    private float speedDeplacementRunning = 12f; // Vitesse quand le joueur court
    private float multipleSpeedDeplacementBackwardAndSide = 0.4f; // Ralentissement marche arrière/côté
    private float speedRotation = 100f; // Vitesse de rotation (gauche/droite)
    private float jumpForce = 5f; // Force appliquée au saut

    [Header("Collision")]
    [SerializeField] private LayerMask groundMask; // Layer pour détecter le sol

    [Header("Components")]
    [SerializeField] private Animator anim; // Animator pour gérer les animations
    private Transform groundCheck; // Point de référence pour checker le sol
    private Rigidbody rb; // Rigidbody du joueur

    [SerializeField] private PlayerInputs inputs; // Référence au script des inputs
    #endregion

    #region Private State
    private bool isGrounded;
    private bool wasGrounded;
    private bool isFalling;
    private bool canMove = true;
    private bool canJump = true;

    private float horizontal;
    private float vertical;
    private Vector3 inputVector;
    private float currentSpeed;
    private bool moving;
    private float rotationVelocity = 0f;
    private bool isHoldingBow = false;
    private bool jumpReady = true;

    [HideInInspector] public bool airAttackRequested = false;
    #endregion

    #region Unity Methods
    /// <summary>
    /// Initialise le Rigidbody et le point de détection du sol.
    /// </summary>
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
    }

    /// <summary>
    /// Méthode appelée chaque frame pour lire les inputs, gérer les collisions, le mouvement, la rotation et les animations.
    /// </summary>
    void Update()
    {
        ReadInputs();
        UpdateCollisionStatus();
        CalculateMovement();
        HandleRotation();
        HandleAnimations();

        if (inputs.IsJumpPressed())
            TryToJump();
    }

    /// <summary>
    /// Méthode appelée à chaque frame physique pour appliquer le mouvement.
    /// </summary>
    private void FixedUpdate()
    {
        ApplyMovement();
    }
    #endregion

    #region Input Handling
    /// <summary>
    /// Lit les inputs du joueur via le script PlayerInputs.
    /// </summary>
    private void ReadInputs()
    {
        horizontal = inputs.GetHorizontal();
        vertical = inputs.GetVertical();

        isFalling = rb.linearVelocity.y < -0.1f && !isGrounded;
    }

    /// <summary>
    /// Définit si le joueur tient l'arc.
    /// </summary>
    /// <param name="value">True si le joueur tient l'arc, false sinon.</param>
    public void SetBowHold(bool value)
    {
        isHoldingBow = value;
    }
    #endregion

    #region Collision
    /// <summary>
    /// Vérifie si le joueur touche le sol et gère les atterrissages.
    /// </summary>
    private void UpdateCollisionStatus()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);

        if (!wasGrounded && isGrounded)
            OnLand();
    }

    /// <summary>
    /// Appelé lorsque le joueur atterrit après un saut ou une chute.
    /// </summary>
    private void OnLand()
    {
        if (airAttackRequested)
        {
            anim.SetTrigger("AttackAirFall");
            airAttackRequested = false;
        }
        canMove = true;
    }
    #endregion

    #region Movement Logic
    /// <summary>
    /// Calcule la direction et la vitesse de déplacement selon les inputs.
    /// </summary>
    private void CalculateMovement()
    {
        if (!canMove)
        { 
            inputVector = Vector3.zero; 
            moving = false; 
            return; 
        }
        
        float adjustedVertical = vertical < 0 ? vertical * multipleSpeedDeplacementBackwardAndSide : vertical;
        float sideMultiplier = isHoldingBow ? multipleSpeedDeplacementBackwardAndSide : 1f;

        inputVector = new Vector3(horizontal * sideMultiplier, 0, adjustedVertical * sideMultiplier);
        moving = inputVector.magnitude > 0.1f;
        HandleSprint();
    }

    /// <summary>
    /// Gère la vitesse et l'état de course selon les inputs.
    /// </summary>
    private void HandleSprint()
    {
        if (!moving || inputs.IsJumpPressed())
        {
            currentSpeed = speedDeplacement;
            anim.SetBool("isRunning", false);
            return;
        }

        if (inputs.IsRunPressed() && vertical > 0)
        {
            currentSpeed = speedDeplacementRunning;
            anim.SetBool("isRunning", true);
        }
        else
        {
            currentSpeed = speedDeplacement;
            anim.SetBool("isRunning", false);
        }
    }

    /// <summary>
    /// Applique le mouvement calculé au Rigidbody.
    /// </summary>
    private void ApplyMovement()
    {
        if (!moving) return;
        Vector3 move = transform.TransformDirection(inputVector) * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    /// <summary>
    /// Gère la rotation du joueur selon l'input souris.
    /// </summary>
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
    /// <summary>
    /// Tente de faire sauter le joueur si possible.
    /// </summary>
    private void TryToJump()
    {
        if (!isGrounded || !canJump) return;

        anim.ResetTrigger("SwordAttack");
        anim.ResetTrigger("SwordAttackInAir");
        anim.ResetTrigger("AttackAirFall");

        jumpReady = false;
        anim.SetTrigger("Jump");
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        Invoke(nameof(ResetJumpReady), 0.1f);
    }

    private void ResetJumpReady() => jumpReady = true;
    #endregion

    #region Animations
    /// <summary>
    /// Met à jour les paramètres de l'Animator selon le mouvement et la chute.
    /// </summary>
    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", inputVector.x, 0.1f, Time.deltaTime);
        anim.SetFloat("zVelocity", inputVector.z, 0.1f, Time.deltaTime);
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("isMoving", moving);
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Active ou désactive la possibilité de se déplacer et de sauter.
    /// </summary>
    /// <param name="enable">True pour autoriser le mouvement et le saut, false pour les bloquer.</param>
    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }

    public void ResetJumpTrigger()
    {
        anim.ResetTrigger("Jump");
    }
    #endregion

    #region Getters
    /// <summary>
    /// Indique si le joueur touche le sol.
    /// </summary>
    public bool IsGrounded() { return isGrounded; }
    #endregion
}
