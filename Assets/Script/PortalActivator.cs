using UnityEngine;

public class PortalActivator : MonoBehaviour
{
    public GameObject portal;
    public GameObject globalVolume;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            globalVolume.SetActive(true);
            portal.SetActive(true);
        }
    }

}
