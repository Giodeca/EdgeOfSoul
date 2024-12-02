using System.Collections;
using UnityEngine;

public class CollonninaPhoto : MonoBehaviour
{

    [Tooltip("The camera for DaVinci Camera")]
    public CamTest m_Camera;
    //public CamTest m_Camera2;

    [Tooltip("The photo for DaVinci Camera")]
    public Photos m_Photo;
    //public Photos m_Photo2;
    public GameObject PhotoToMove;


    public bool IsAiming { get; private set; }
    public int ActiveItemIndex { get; private set; }

    public bool MakeThePhoto;
    public bool SecondPhotoDone;

    [SerializeField] private float raycastDInstance = 100f;
    public LayerMask raycastLayerMask;
    public GameObject flash;
    public Transform PhotoPositionEnd;
    public MeshCollider colliderPhoto;
    public Rigidbody RigidbodyPhoto;
    public GameObject TextPhotoNotTake;
    public GameObject TextInt;
    public bool isForCage;
    private AudioSource audioSource;

    [SerializeField] private GameObject PhotoTexture;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cameraPlayer;
    [SerializeField] private GameObject canvasText;
    [SerializeField] private GameObject newPoint;
    [SerializeField] private LayerMask layerPhoto;


    private void OnEnable()
    {
        EventManager.ColonninaPhoto += OnColonninaPhoto;
    }
    private void OnDisable()
    {
        EventManager.ColonninaPhoto -= OnColonninaPhoto;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ActiveItemIndex = 0;
        m_Camera.ShowItem(false);
        m_Photo.ShowItem(false);
        GetActiveItem().ShowItem(true);
    }
    private void OnColonninaPhoto()
    {
        StartCoroutine(PhotoCoroutine());
    }
    public void TakePhoto()
    {
        Vector3 forewardDirection = flash.transform.forward;
        Vector3 startDirection = Quaternion.Euler(0, -45, 0) * forewardDirection;
        float angleIncrement = 10f;

        for (int i = 0; i <= 8; i++)
        {
            Vector3 direction = Quaternion.Euler(0, i * angleIncrement, 0) * startDirection;

            RaycastHit hit;
            if (Physics.Raycast(flash.transform.position, direction, out hit, raycastDInstance, raycastLayerMask))
            {
                PlayerMovement.Instance.hasBeenPhotoGraph = true;

                if (isForCage)
                    PlayerMovement.Instance.canTeleportInCube = true;

                var activeItem = GetActiveItem();
                if (activeItem == null)
                    return;
                if (activeItem.TryUse())
                {
                    SwitchItem();
                    MakeThePhoto = false;
                }
                Debug.DrawRay(flash.transform.position, direction * raycastDInstance, Color.red);
            }
            else
            {

                Debug.DrawRay(flash.transform.position, direction * raycastDInstance, Color.green);
            }

        }

    }
    //IEnumerator ShowTextInt()
    //{
    //    TextInt.SetActive(true);
    //    yield return new WaitForSeconds(2);
    //    TextInt.SetActive(false);
    //}
    //IEnumerator ShowTextWrong()
    //{
    //    TextPhotoNotTake.SetActive(true);
    //    yield return new WaitForSeconds(2);
    //    TextPhotoNotTake.SetActive(false);
    //}
    public void SwitchItem()
    {
        GetActiveItem().ShowItem(false);
        int v = ActiveItemIndex == 0 ? 1 : 0;
        ActiveItemIndex = v;
        GetActiveItem().ShowItem(true);
    }


    IEnumerator PhotoCoroutine()
    {

        PlayerMovement.Instance.isWaitingForPhoto = true;
        flash.SetActive(true);
        yield return new WaitForSeconds(2);
        flash.SetActive(false);
        yield return new WaitForSeconds(1);
        flash.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        flash.SetActive(false);
        audioSource.Play();
        TakePhoto();
        GameObject newPhoto = Instantiate(PhotoToMove, PhotoToMove.transform.position, Quaternion.identity);

        if (!isForCage)
        {
            newPhoto.GetComponent<InstanceTeleport>().Initialized(player, cameraPlayer, canvasText, newPoint);
            newPhoto.layer = (int)Mathf.Log(layerPhoto.value, 2); ;
        }
        else if (isForCage)
        {

            newPhoto.GetComponent<PhotoCubeTeleport>().enabled = true;
            newPhoto.GetComponent<PhotoCubeTeleport>().mainCam = cameraPlayer;
            newPhoto.layer = (int)Mathf.Log(layerPhoto.value, 2); ;

        }

        //if (PlayerMovement.Instance.hasBeenPhotoGraph == true)
        StartCoroutine(PhotoGraph(newPhoto, newPhoto.transform, PhotoPositionEnd));


        // Attiva tutti i child di newPhoto
        foreach (Transform child in newPhoto.transform)
        {
            child.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2f);
        if (!isForCage)
            newPhoto.GetComponent<InstanceTeleport>().canDestroy = true;


        PlayerMovement.Instance.isWaitingForPhoto = false;


    }
    IEnumerator PhotoGraph(GameObject photo, Transform startingPos, Transform endPos)
    {
        float duration = 2;
        float elapseTime = 0f;

        Vector3 startingPosition = startingPos.transform.position;

        while (elapseTime < duration)
        {
            startingPos.position = Vector3.Lerp(startingPosition, endPos.position, elapseTime / duration);
            elapseTime += Time.deltaTime;
            yield return null;
        }
        startingPos.position = endPos.position;
        photo.GetComponent<MeshCollider>().enabled = true;
    }

    public ItemCollector GetActiveItem()
    {
        return ActiveItemIndex == 0 ? m_Camera : m_Photo;
    }



}
