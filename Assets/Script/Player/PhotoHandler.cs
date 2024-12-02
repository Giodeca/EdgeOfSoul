using UnityEngine;
public enum PositionType
{
    Default,
    Aiming,
    Down
}
public class PhotoHandler : MonoBehaviour
{

    [Tooltip("The camera for DaVinci Camera")]
    public CamTest m_Camera;

    [Tooltip("The photo for DaVinci Camera")]
    public Photos m_Photo;

    [Header("References")]

    [Tooltip("Parent transform where all items will be added in the hierarchy")]
    public Transform ItemParentSocket;

    [Tooltip("Position for items when active but not actively aiming")]
    public Transform DefaultItemPosition;

    [Tooltip("Position for items when aiming")]
    public Transform AimingItemPosition;

    [Tooltip("Position for inactive items")]
    public Transform DownItemPosition;

    public float AimingAnimationSpeed = 0.3f;

    private int PhotoCount;
    public bool IsAiming { get; private set; }
    public int ActiveItemIndex { get; private set; }

    public GameObject canvas;
    public GameObject aim;
    public bool PhotoHasBeDone;


    void Start()
    {
        ActiveItemIndex = 0;

        m_Camera.ShowItem(false);
        m_Photo.ShowItem(false);

        GetActiveItem().ShowItem(true);
        // ItemParentSocket.position = DownItemPosition.position;
        ItemParentSocket.position = AimingItemPosition.position;
    }


    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (PlayerMovement.Instance.CanMakePhoto == true)
            {
                aim.SetActive(true);
                LerpPosition();
                if (Input.GetMouseButton(0))
                {
                    canvas.SetActive(true);
                    aim.SetActive(false);
                    IsAiming = true;
                }
                else
                {
                    canvas.SetActive(false);
                    aim.SetActive(true);
                    IsAiming = false;
                }


                var activeItem = GetActiveItem();
                if (activeItem == null)
                    return;

                if (IsAiming && Input.GetKeyDown(KeyCode.K) && PlayerMovement.Instance.PhotoCount <= 9)
                {
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.photoCameraSound);
                    PlayerMovement.Instance.PhotoCount++;
                    Debug.Log("PhotoCount");
                    if (activeItem.TryUse())
                        SwitchItem();
                }
            }
        }
        else
        {
            if (aim != null)
                aim.SetActive(false);
        }

    }





    void LerpPosition()
    {
        Vector3 destinationPos = DefaultItemPosition.localPosition;

        if (IsAiming)
            destinationPos = AimingItemPosition.localPosition + GetActiveItem().AimOffset;

        float movementDelta = Time.deltaTime * AimingAnimationSpeed;
        Vector3 newLocation = Vector3.MoveTowards(ItemParentSocket.localPosition, destinationPos, movementDelta);

        if (newLocation != ItemParentSocket.localPosition)
        {
            ItemParentSocket.localPosition = newLocation;
        }
    }

    // Iterate on all item slots to find the next valid item to switch to
    public void SwitchItem()
    {

        if (!IsAiming)
            return;
        ItemParentSocket.position = DownItemPosition.position;
        IsAiming = false;
        GetActiveItem().ShowItem(false);
        int v = ActiveItemIndex == 0 ? 1 : 0;
        ActiveItemIndex = v;
        GetActiveItem().ShowItem(true);
    }

    // Adds an item to our inventory


    public ItemCollector GetActiveItem()
    {
        return ActiveItemIndex == 0 ? m_Camera : m_Photo;
    }
}

