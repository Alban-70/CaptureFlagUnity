using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField] private TakeArrowBloc parent;


    private void OnTriggerEnter(Collider other)
    {
        parent.ChildTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        parent.ChildTriggerExit(other);
    }
}
