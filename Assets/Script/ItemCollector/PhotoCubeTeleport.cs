using UnityEngine;

public class PhotoCubeTeleport : MonoBehaviour
{
    public GameObject mainCam;

    private void Update()
    {
        if (transform.parent == mainCam.transform)
        {

            PlayerMovement.Instance.AllowCubeTp = true;
            Debug.Log(PlayerMovement.Instance.AllowCubeTp);
        }
        else
        {
            PlayerMovement.Instance.AllowCubeTp = false;
        }
    }
}
