using UnityEngine;

public class WeightScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject doorToOpen;
    [SerializeField] private float limitWight;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
        {

            float weightSave = collision.gameObject.GetComponent<WeightObject>().weight;

            if (weightSave >= limitWight)
            {
                doorToOpen.GetComponent<RotateDoor>().Open(player.transform.position);
            }


        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
        {
            doorToOpen.SetActive(true);
        }
    }
}
