using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask playerLayer;

    private Transform playerTransform;
    private float speed = 5f;
    private Rigidbody rb;
    private Vector3 inputVector;
    private bool followPlayer = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (player != null)
            playerTransform = player.transform;
        else
            Debug.LogError("Player n'est pas assigné !");
    }

    void Update()
    {
        FollowPlayer();
    }

    void FixedUpdate()
    {
        if (inputVector.magnitude > 0.1f)
        {
            Vector3 move = transform.TransformDirection(inputVector) * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }

    private void FollowPlayer()
    {
        // if (playerTransform != null && followPlayer)
        // {
        //     Vector3 direction = playerTransform.position - transform.position;
        //     direction.y = 0;
        //     inputVector = direction.normalized;
        // }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
        {
            inputVector = Vector3.zero;
            followPlayer = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
        {
            followPlayer = true;
        }
    }
}
