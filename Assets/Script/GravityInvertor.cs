using UnityEngine;

public class GravityInvertor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement.Instance.GravityInverted = true;
        }
    }
}
