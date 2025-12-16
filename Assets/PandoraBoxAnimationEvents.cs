using UnityEngine;

public class PandoraBoxAnimationEvents : MonoBehaviour
{
    [SerializeField] private ShowZone showZone;


    public void DestroyPandoraBox() => showZone.DestroyPandoraBox();
}
