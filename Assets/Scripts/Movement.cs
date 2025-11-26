using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float speedDeplacement = 7f;
    [SerializeField] private float speedRotation = 3f;


    private Rigidbody rb;
    [SerializeField] private Animator anim;
    private Vector3 inputVector; // Stocke l’input pour FixedUpdate


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Récupération de l'input
        // float horizontal = Input.GetAxisRaw("Horizontal");       // permet d'avoir que -1 ou 0 ou 1, pas d'entre deux
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputVector = new Vector3(horizontal, 0, vertical);
        
        bool moving = inputVector.magnitude > 0.1f;
        anim.SetBool("isMoving", moving);

        anim.SetFloat("xVelocity", inputVector.x, 0.1f, Time.deltaTime);
        anim.SetFloat("yVelocity", inputVector.z, 0.1f, Time.deltaTime);

        // Pour que la vitesse soit constante en diagonale
        if(inputVector.magnitude > 1f)
            inputVector.Normalize();

        PlayerOrientation();
    }

    // 50 fois par seconde (évite de perdre des fps avec les calculs)
    private void FixedUpdate()
    {
        // S'occupe du déplacement du personnage
        if(inputVector.magnitude > 0.1f)
        {
            Vector3 move = inputVector * speedDeplacement * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + transform.TransformDirection(move));
        }
    }

    void PlayerOrientation()
    {
        if (Input.GetKey(KeyCode.A)) transform.Rotate(0, -speedRotation * Time.deltaTime, 0); 
        if (Input.GetKey(KeyCode.E)) transform.Rotate(0, speedRotation * Time.deltaTime, 0);
    }
}
