using UnityEngine;

public class DestoyPhoto : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.gameObject.CompareTag("Player"))
        {
            EventManager.DestroyPhoto?.Invoke();
        }
    }
}
