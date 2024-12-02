using UnityEngine;

public class RepawnScriptTPCube : MonoBehaviour
{
    public GameObject blockWall;
    public GameObject DeactivateTo;
    public GameObject Activate;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (blockWall != null)
                blockWall.SetActive(true);

            Activate.SetActive(true);
            DeactivateTo.SetActive(false);
            PlayerMovement.Instance.PhotoCount = 0;
            EventManager.SavePosition?.Invoke(transform.position);

        }

    }
}
