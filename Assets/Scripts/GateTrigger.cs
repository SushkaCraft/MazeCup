using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    [SerializeField] GameObject _gate;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cup")) DestroyGate();
    }

    private void DestroyGate() => Destroy(_gate);
    
}
