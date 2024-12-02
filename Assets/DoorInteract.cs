using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public LayerMask door;
    public LayerMask door2;
    private RaycastHit hit;
    private void Update()
    {
        if (!PauseMenu.isPaused)
            ActivateDoor();
    }

    private void ActivateDoor()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 20f, door))
        {
            if (Input.GetKeyDown(KeyCode.E) && hit.collider.TryGetComponent(out RotateDoor door))
            {
                door.Open(transform.position);
            }
        }
    }
}
