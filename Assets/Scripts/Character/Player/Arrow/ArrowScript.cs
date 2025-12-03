using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;

    private Rigidbody boxRb;
    private BoxCollider boxCollider;

    void Awake()
    {
        boxRb = gameObject.GetComponent<Rigidbody>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    private void StickyOnTarget(Transform target)
    {
        boxRb.linearVelocity = Vector3.zero;
        boxRb.angularVelocity = Vector3.zero;
        
        transform.SetParent(target);
        Debug.Log("Flèche plantée dans l'ennemi !");
    }

    private void UnabledRigidAndBox()
    {
        boxRb.isKinematic = true;
        boxCollider.isTrigger = true;
        // boxCollider.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(((1 << collision.gameObject.layer) & enemyLayer.value) != 0)
        {
            StickyOnTarget(collision.transform);
            UnabledRigidAndBox();
        }
    }
}
