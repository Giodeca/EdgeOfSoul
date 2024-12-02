using UnityEngine;

public class PortalCounter : MonoBehaviour
{
    private bool isUpdated;
    [SerializeField] Animator animator;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
        {
            if (!isUpdated)
            {
                animator.SetBool("isDown", true);
                PlayerMovement.Instance.CubeCount++;
                isUpdated = true;
            }
        }
        Debug.Log(PlayerMovement.Instance.CubeCount);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
        {
            if (isUpdated)
            {
                animator.SetBool("isDown", false);
                PlayerMovement.Instance.CubeCount--;
                isUpdated = false;
            }
        }
    }
}
