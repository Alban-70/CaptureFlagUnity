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
    private Transform groundCheck;
    private Rigidbody rb;
    private Vector3 inputVector; // Stocke l’input pour FixedUpdate

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
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    #region Public methods 
    public void EnableMovementAndJump(bool enable)
    {
        Debug.Log("enable : " + enable);
        canMove = enable;
        canJump = enable;
    }
    #endregion

    #region Input & Collision
    private void GetPlayerInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            TryToJump();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            TryToAttack();
    }

    private void UpdateCollisionStatus()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
    }
    #endregion

    #region Movement
    private void CalculateMovement()
    {
        CalculateInputVector();
        moving = inputVector.magnitude > 0.1f;
        HandleSprint();
    }

    private void CalculateInputVector()
    {
        if (canMove)
        {
            float adjustedVertical = vertical < 0 ? vertical * multipleSpeedDeplacementBackwardAndSide : vertical;
            inputVector = new Vector3(horizontal * multipleSpeedDeplacementBackwardAndSide, 0, adjustedVertical);
        } else
        {
            inputVector = Vector3.zero;
        }
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
        if (inputVector.magnitude > 0.1f)
        {
            Vector3 move = transform.TransformDirection(inputVector) * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.A)) transform.Rotate(0, -speedRotation * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(0, speedRotation * Time.deltaTime, 0);
    }
    #endregion

    #region Actions
    private void TryToJump()
    {
        if (isGrounded && canJump)
        {
            anim.SetTrigger("Jump");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    private void TryToAttack()
    {
        if (isGrounded)
        {
            anim.SetTrigger("Attack");
        }
    }
    #endregion

    #region Animations
    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", inputVector.x, 0.1f, Time.deltaTime);
        anim.SetFloat("yVelocity", inputVector.z, 0.1f, Time.deltaTime);
        anim.SetBool("isMoving", moving);
    }
    #endregion
}
