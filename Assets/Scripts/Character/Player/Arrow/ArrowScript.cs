using System.Collections;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{

    void Awake()
    {
        
    }

    private IEnumerator UnFreezeAfterDelay(Rigidbody enemyRb, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (enemyRb != null)
            enemyRb.constraints = RigidbodyConstraints.FreezeRotation;
    }



    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("Touché");
                
                if (enemyRb != null)
                {
                    enemyRb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                    StartCoroutine(UnFreezeAfterDelay(enemyRb, 0.5f));
                }
            }
            Destroy(gameObject);
        }
    }
}
