using UnityEngine;

public class WalkcablePlatform : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private LayerMask maskPickable;
    [SerializeField] private LayerMask maskTrasparent;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
            obj.SetActive(true);
        //obj.layer = (int)Mathf.Log(maskPickable.value, 2);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
            obj.SetActive(false);
        //obj.layer = (int)Mathf.Log(maskTrasparent.value, 2);

    }
}
