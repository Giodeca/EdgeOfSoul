using UnityEngine;

public class AnimationWallLever : MonoBehaviour
{
    [SerializeField] Animator animator;
    private void OnEnable()
    {
        EventManager.WallLeverDoorAnim += OnWallLeverAnimation;
    }

    private void OnDisable()
    {
        EventManager.WallLeverDoorAnim -= OnWallLeverAnimation;
    }


    public void ActivateAnim()
    {
        animator.SetTrigger("LeverAction");
    }
    private void OnWallLeverAnimation()
    {

    }
}
