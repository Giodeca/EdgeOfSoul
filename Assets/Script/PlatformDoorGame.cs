using UnityEngine;

public class PlatformDoorGame : MonoBehaviour
{
    [SerializeField] Animator animator;
    private bool isUpdated;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
        {
            if (!isUpdated)
            {
                animator.SetBool("isDown", true);
                PlayerMovement.Instance.platformCount++;
                isUpdated = true;
            }
        }
        Debug.Log(PlayerMovement.Instance.platformCount);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("IPickable"))
        {
            if (isUpdated)
            {
                animator.SetBool("isDown", false);
                PlayerMovement.Instance.platformCount--;
                isUpdated = false;
            }
        }
    }
}
