using UnityEngine;

public class TurningPC : MonoBehaviour
{
    [SerializeField] private GameObject pcPanel;
    [SerializeField] private Animator animator;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
        {
            PlayerMovement.Instance.canUsePC = true;
            animator.SetBool("isCubeOver", true);
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
        {
            PlayerMovement.Instance.canUsePC = false;
            animator.SetBool("isCubeOver", false);
        }

    }
}
