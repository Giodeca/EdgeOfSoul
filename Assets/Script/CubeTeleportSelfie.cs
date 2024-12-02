using UnityEngine;

public class CubeTeleportSelfie : MonoBehaviour
{
    public bool canTPhere;
    public GameObject player;
    public Transform tpPos;
    private void OnEnable()
    {
        EventManager.TeleportInCube += OnTeleportCube;
    }

    private void OnDisable()
    {
        EventManager.TeleportInCube -= OnTeleportCube;
    }


    private void OnTeleportCube()
    {
        if (canTPhere)
        {
            PlayerMovement.Instance.controller.enabled = false;
            player.transform.position = tpPos.position;
            PlayerMovement.Instance.controller.enabled = true;
            canTPhere = false;
        }

    }
}
