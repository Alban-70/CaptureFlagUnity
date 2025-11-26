using Unity.Mathematics;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float speedDeplacement = 7f;
    [SerializeField] private float speedDeplacementBackwardAndSide = 3f;
    [SerializeField] private float speedDeplacementRunning = 12f;
    [SerializeField] private float speedRotation = 3f;
    [SerializeField] private LayerMask groundMask;

    private float currentSpeed;
    private bool isGrounded;
    private Transform groundCheck;
    private Rigidbody rb;
    [SerializeField] private Animator anim;
    private Vector3 inputVector; // Stocke l’input pour FixedUpdate


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
        if (groundCheck == null)
            Debug.Log("GroundCheck est null");
        Debug.Log("GroundCheck is not null !");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerOrientation();
        PlayerMovement();

        // Saut
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
        PlayerJump();
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

    private void PlayerMovement()
    {
        // Inputs
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // Pour ralentir le personnage 
        if (vertical < 0) 
            vertical = speedDeplacementBackwardAndSide;
        
        inputVector = new Vector3(speedDeplacementBackwardAndSide, 0, vertical);

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

    private void PlayerOrientation()
    {
        if (Input.GetKey(KeyCode.A)) transform.Rotate(0, -speedRotation * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.E)) transform.Rotate(0, speedRotation * Time.deltaTime, 0);
    }

    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            anim.SetTrigger("Jump");
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }


}
