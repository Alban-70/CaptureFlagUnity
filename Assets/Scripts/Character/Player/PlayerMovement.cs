using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    

    [Header("Player Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float speedDeplacement = 7f;
    [SerializeField] private float multipleSpeedDeplacementBackwardAndSide = 0.4f;
    [SerializeField] private float speedDeplacementRunning = 12f;
    [SerializeField] private float speedRotation = 40f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Animator anim;

    // Movements
    private float horizontal;
    private float vertical;
    private float currentSpeed;


    private bool isAttacking = false;
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

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleOrientation();
        HandleMovement();

        // Saut
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);   
    }

    // 50 fois par seconde (évite de perdre des fps avec les calculs)
    private void FixedUpdate()
    {
        // S'occupe du déplacement du personnage
        if (inputVector.magnitude > 0.1f)
        {
            Vector3 move = transform.TransformDirection(inputVector) * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
        
    }

    private IEnumerator WaitForAttackAnimation()
    {
        isAttacking = true;
        // On attend que l'animation Attack commence
        yield return null;

        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }

        // On attend que l'animation Attack soit terminée
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }
        isAttacking = false;
    }


    private void HandleInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            TryToJump();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            TryToAttack();
    }

    private void HandleMovement()
    {
        if (isAttacking) return;

        // Pour ralentir le personnage 
        if (vertical < 0) 
            vertical *= multipleSpeedDeplacementBackwardAndSide;
        
        inputVector = new Vector3(horizontal * multipleSpeedDeplacementBackwardAndSide, 0, vertical);

        bool moving = inputVector.magnitude > 0.1f;
        anim.SetBool("isMoving", moving);

        anim.SetFloat("xVelocity", inputVector.x, 0.1f, Time.deltaTime);
        anim.SetFloat("yVelocity", inputVector.z, 0.1f, Time.deltaTime);

        // Sprint
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

    private void HandleOrientation()
    {
        if (Input.GetKey(KeyCode.A)) transform.Rotate(0, -speedRotation * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(0, speedRotation * Time.deltaTime, 0);
    }

    private void TryToJump()
    {
        if (isAttacking) return;

        if (isGrounded)
        {
            anim.SetTrigger("Jump");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }

    private void TryToAttack()
    {
        if (isGrounded && horizontal == 0 && vertical == 0 && !isAttacking)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(WaitForAttackAnimation());
        }
    }


}
