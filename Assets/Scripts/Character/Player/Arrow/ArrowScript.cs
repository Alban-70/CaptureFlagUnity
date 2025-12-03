using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;

    private BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void UnableBoxCollider()
    {
        boxCollider.enabled = false;
        Debug.Log("box collider désactivé");
    }

    void OnCollisionEnter(Collision collision)
    {
        if(((1 << collision.gameObject.layer) & enemyLayer.value) != 0)
        {
            Debug.Log("Arrow enter");
            UnableBoxCollider();
        }
    }
}
