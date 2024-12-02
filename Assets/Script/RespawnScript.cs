using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public GameObject blockWall;
    public GameObject mapToDestroy;
    public bool resetPhoto;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (blockWall != null)
                blockWall.SetActive(true);


            PlayerMovement.Instance.PhotoCount = 0;
            if (resetPhoto)
            {
                PlayerMovement.Instance.CanMakePhoto = true;
            }
            EventManager.SavePosition?.Invoke(transform.position);

            if (mapToDestroy != null)
                mapToDestroy.SetActive(false);

        }

    }
}
