using UnityEngine;

public class WeightElevator : MonoBehaviour
{

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
        {
            EventManager.ElevetorOn?.Invoke();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
        {
            EventManager.ElevetorOff?.Invoke();
        }
    }
}
