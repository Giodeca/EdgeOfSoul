using UnityEngine;

public class InstanceTeleport : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraPlayer;
    [SerializeField] private GameObject canvasText;
    [SerializeField] private LayerMask cage;
    [SerializeField] private GameObject newPoint;
    [SerializeField] private bool canInstance;
    public bool canDestroy;


    private void OnEnable()
    {
        EventManager.TeleportInCage += OnTeleport;
    }
    private void OnDisable()
    {
        EventManager.TeleportInCage -= OnTeleport;
    }
    private void Update()
    {

        Debug.Log(transform.parent);
        if (this.transform.parent == cameraPlayer.transform)
        {
            Debug.Log("Padre");
            if (PlayerMovement.Instance.hasBeenPhotoGraph == true)
            {
                RaycastHit hit;
                if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, Mathf.Infinity, cage))
                {
                    canvasText.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        canvasText.SetActive(false);
                        Debug.Log("Going In the cage");
                        EventManager.TeleportInCage?.Invoke();
                    }
                    Debug.DrawRay(cameraPlayer.transform.position, cameraPlayer.transform.forward * hit.distance, Color.red);
                }
                else
                {
                    canvasText.SetActive(false);
                }
            }
            else
            {
                Debug.DrawRay(cameraPlayer.transform.position, cameraPlayer.transform.forward * 1000f, Color.green);
            }
        }
        else
        {
            // Se il parent non è il player, disattiva il canvasText
            canvasText.SetActive(false);
        }
    }
    public void Initialized(GameObject playetTo, GameObject cameraPhoto, GameObject canvasTexto, GameObject newPointz)
    {
        player = playetTo;
        cameraPlayer = cameraPhoto;
        canvasText = canvasTexto;
        newPoint = newPointz;
    }
    private void OnTeleport()
    {
        if (canDestroy)
            Destroy(gameObject);
    }
}

