using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Transform player;
    void Start()
    {
        
    }

    void Update()
    {
        
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
