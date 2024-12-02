using UnityEngine;

public class FinalScriptEndGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerMovement.Instance.isInFinalPlace = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            PlayerMovement.Instance.isInFinalPlace = false;
    }
}
