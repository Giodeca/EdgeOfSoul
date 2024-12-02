using UnityEngine;

public class CanOpenCage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerMovement.Instance.allowExit = true;

    }
}
