using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement details")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float speedDeplacement = 7f;
    [SerializeField] private float multipleSpeedDeplacementBackwardAndSide = 0.4f;
    [SerializeField] private float speedDeplacementRunning = 12f;
    [SerializeField] private float speedRotation = 40f;

    private bool isFalling;
    private bool airAttackRequested = false; 
    private bool continueAirAttack = false;  

    private float horizontal;
    private float vertical;
    private float currentSpeed;
    private bool moving;

    private bool canMove = true;
    private bool canJump = true;

    [Header("Collision details")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Animator anim;

    private bool isGrounded;
    private bool wasGrounded;
    private Transform groundCheck;
    private Rigidbody rb;
    private Vector3 inputVector;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
        if (groundCheck == null)
            Debug.Log("GroundCheck est null");
    }

    void Update()
    {
        UpdateCollisionStatus();
        GetPlayerInput();
        CalculateMovement();
        HandleAnimations();
        HandleRotation();

        // Détection descente après avoir attaqué en l'air
        if (airAttackRequested && rb.linearVelocity.y < 0)
        {
            continueAirAttack = true;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    #region Input & Collision
    private void GetPlayerInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        isFalling = rb.linearVelocity.y < -0.1f && !isGrounded;

        if (Input.GetKeyDown(KeyCode.Space))
            TryToJump();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isGrounded)
                TryToAttackInAir();
            else
                TryToAttackInGround();
        }
    }

    private void UpdateCollisionStatus()
    {
        wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);

        // détection de l'atterrissage
        if (!wasGrounded && isGrounded)
            OnLand();
    }

    private void OnLand()
    {
        if (!airAttackRequested) return;

        anim.SetTrigger("AttackAirFall");

        airAttackRequested = false;
        continueAirAttack = false;

        canMove = true;
    }
    #endregion

    #region Actions
    private void TryToJump()
    {
        if (!isGrounded || !canJump) return;

        anim.SetTrigger("Jump");
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    private void TryToAttackInGround()
    {
        anim.SetTrigger("Attack");
    }

    private void TryToAttackInAir()
    {
        anim.SetTrigger("AttackInAir");

        canMove = false;
        anim.applyRootMotion = false;
        airAttackRequested = true;
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
        anim.applyRootMotion = true;
    } 

    public void PrepareAirAttackFall() => airAttackRequested = true;
    #endregion

    #region Movement
    private void CalculateMovement()
    {
        if (canMove)
        {
            float adjustedVertical =
                vertical < 0 ? vertical * multipleSpeedDeplacementBackwardAndSide : vertical;

            inputVector = new Vector3(
                horizontal * multipleSpeedDeplacementBackwardAndSide,
                0,
                adjustedVertical
            );
        }
        else inputVector = Vector3.zero;

        moving = inputVector.magnitude > 0.1f;

        HandleSprint();
    }

    private void HandleSprint()
    {
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

            rb.MovePosition(rb.position + move);
        }
    }

    private void HandleRotation()
    {
        if (!canMove) return;

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -speedRotation * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.E))
            transform.Rotate(0, speedRotation * Time.deltaTime, 0);
    }
    #endregion


    #region Animations
    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", inputVector.x, 0.1f, Time.deltaTime);
        anim.SetFloat("zVelocity", inputVector.z, 0.1f, Time.deltaTime);
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("isMoving", moving);
    }
    #endregion
}
