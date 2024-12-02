using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public Vector3 respawnPosition;
    public bool StopRotatiom;
    [Header("Movement")]
    public CharacterController controller;
    [SerializeField] private float speed = 12f;
    [SerializeField] Vector3 velocity;
    public float gravity = -9.81f;
    [SerializeField] private Volume volume;
    [Header("PickUp")]
    public GameObject textPickUp;
    public GameObject cameraPlayer;
    [SerializeField] private LayerMask ObjectMask;
    public bool HasBeenPicked;
    public bool isPhotographed;

    public bool CanMakePhoto;
    public bool isUsingPC;
    public bool canUsePC;

    [Header("Jumo&Gravity")]
    [SerializeField] private Transform groundCheck;
    public bool isGrounded;
    public bool GravityInverted;
    public bool isElevate;

    [SerializeField] float jumpHeight;

    [SerializeField] float rayLenght;


    [Header("Cage")]
    [SerializeField] Transform cageTeleport;
    [SerializeField] private LayerMask Activator;
    [SerializeField] private GameObject keyActivator;
    [SerializeField] private GameObject CameraToPickUp;
    [SerializeField] private GameObject colonninaTextActivator;
    [SerializeField] private GameObject TeleportCubeTextActivator;
    public GameObject TextComputer;

    [SerializeField] private GameObject doorText;
    [SerializeField] private GameObject elevatorText;
    [SerializeField] private LayerMask colonninaActivation;
    [SerializeField] private LayerMask door;
    [SerializeField] private LayerMask computer;
    [SerializeField] private LayerMask Elevator;
    [SerializeField] private LayerMask allowPhoto;
    [SerializeField] private LayerMask activatorGravity;
    [SerializeField] private LayerMask CubeTeleport;
    [SerializeField] private LayerMask PaperRead;
    public GameObject textGravity;

    public GameObject textPickUp2;
    public GameObject doorToRemoveWithPlat;
    public int platformCount;
    private bool isRotationApplied = false;
    [SerializeField] private GameObject aim;
    public bool hasBeenPhotoGraph;
    public bool isWaitingForPhoto;
    public SpawnManager spawnManager;

    [Header("Refences")]
    public Transform orientation;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;
    public bool climbing;

    [Header("Detection")]
    public float detectonLenght;
    public float sphereCastRadious;
    public float maxWallLockAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    public GameObject portal1ToDestory;
    public GameObject portal2ToDestory;
    public GameObject portal3ToDestory;

    [Header("Text")]
    public GameObject paperToRead;
    public GameObject paperToReadText;



    [Header("DoorGameAnim")]
    public GameObject doorTopenLast;
    public GameObject TextLevaDoor;
    public LayerMask layerLeverDoor;


    public int PhotoCount;

    public bool allowExit;
    public int CubeCount;
    public bool canTeleportInCube;

    [Header("SapwnCube")]
    public int CubeSpawned;
    public GameObject objToSpawn;
    public GameObject TextSpawnCube;
    public LayerMask invisible;

    public LayerMask finalElevator;
    public GameObject textElevatorFinal;
    public bool AllowCubeTp;
    public GameObject finalePos;
    public bool isInFinalPlace;
    protected override void Awake()
    {
        base.Awake();

        //Vignette vignette = volume.profile.TryGet<Vignette>(out vignette);
        EventManager.TeleportInCage += OnTelePortInCage;
        PauseMenu.isPaused = false;

    }
    private void OnDisable()
    {
        EventManager.TeleportInCage -= OnTelePortInCage;
    }
    private void Start()
    {
        aim.SetActive(true);
        transform.position = SpawnManager.Instance.respawnPos;

    }
    void Update()
    {

        if (!PauseMenu.isPaused)
        {
            aim.SetActive(true);

            if (!Instance.isUsingPC)
            {
                Move();
                if (Input.GetButtonDown("Jump") && isGrounded && !GravityInverted)
                    Jump();
                else if (Input.GetButtonDown("Jump") && isGrounded && GravityInverted)
                    JumpInverted();
            }

            //Gravity

            if (GravityInverted)
                ApllyGravityInverted();
            else
                ApllyGravity();



            //Jump
            if (Input.GetKeyDown(KeyCode.P))
                DebugComand();


            //PickUp
            if (Input.GetKeyDown(KeyCode.E))
                EventManager.PickUpObject?.Invoke();

            if (Input.GetKeyDown(KeyCode.N))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            if (!isUsingPC)
            {
                RaycastCheck();

                if (PlayerMovement.Instance.allowExit == true)
                    CageAction();

                if (Instance.isWaitingForPhoto == false)
                    ColonninaActivation();

                FinalActivation();
                ElevatorActivator();
                CameraActivator();
                ActivateGravity();
                PaperReadVoid();
                LastDoorOpen();
                CubeSpawn();
                FinalElevator();

                if (canUsePC)
                    ComputerText();

                if (AllowCubeTp)
                {
                    if (canTeleportInCube)
                        TeleportCube();
                }

            }
            //WallCheck();
            //StateMachine();
            //if (climbing) ClibingMovemnt();
        }
        PlatformGame();
        ChessPortalGame();
    }

    //private void OnPhotoReset()
    //{
    //    PhotoCount = 0;
    //}
    private void DebugComand()
    {
        controller.enabled = false;
        transform.position = finalePos.transform.position;
        controller.enabled = true;
    }
    private void ChessPortalGame()
    {
        if (CubeCount >= 6)
        {
            portal1ToDestory.SetActive(false);
            portal2ToDestory.SetActive(false);
            portal3ToDestory.SetActive(false);
        }
        else
        {
            portal2ToDestory.SetActive(true);
            portal3ToDestory.SetActive(true);
        }
    }
    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        Debug.Log(velocity.y);
    }
    private void JumpInverted()
    {
        velocity.y = -Mathf.Sqrt(jumpHeight * 2 * gravity);
        Debug.Log(velocity.y);
    }

    private void ApllyGravity()
    {
        RaycastHit hit;

        if (Physics.Raycast(groundCheck.position, -groundCheck.up, out hit, rayLenght))
        {
            isGrounded = true;
            if (isGrounded && velocity.y < 0)
                velocity.y = -2;
        }
        else
            isGrounded = false;

        gravity = -9.81f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (isRotationApplied)
        {
            velocity.y = -2;
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
            isRotationApplied = false;
        }

    }

    private void PlatformGame()
    {
        if (platformCount >= 4)
            doorToRemoveWithPlat.GetComponent<RotateDoor>().Open(transform.position);

    }
    private void ApllyGravityInverted()
    {
        RaycastHit hit;
        if (Physics.Raycast(groundCheck.position, -groundCheck.up, out hit, rayLenght))
        {
            isGrounded = true;
            if (isGrounded && velocity.y > 0)
                velocity.y = 2;
        }
        else
            isGrounded = false;



        if (!isRotationApplied)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.y, transform.rotation.y, 180);
            isRotationApplied = true;
        }


        gravity = +9.81f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void CubeSpawn()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 100, invisible))
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                CubeSpawned++;
                if (CubeSpawned >= 10)
                {
                    EventManager.DestroyCube?.Invoke();
                    CubeSpawned = 0;
                }
                GameObject newCube = Instantiate(objToSpawn, hit.point, Quaternion.identity);
            }

        }

    }
    private void CageAction()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 100, Activator))
        {
            keyActivator.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
                EventManager.TeleportOutCage?.Invoke();
        }
        else
            keyActivator.SetActive(false);
    }
    private void FinalElevator()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 5, finalElevator))
        {
            textElevatorFinal.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
                EventManager.FinalElveatorActivationEventOn?.Invoke();
        }
        else
            textElevatorFinal.SetActive(false);
    }
    private void LastDoorOpen()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 5, layerLeverDoor))
        {
            TextLevaDoor.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.collider.gameObject.GetComponent<AnimationWallLever>().ActivateAnim();
                doorTopenLast.GetComponent<RotateDoor>().Open(transform.position);
            }

        }
        else
            TextLevaDoor.SetActive(false);
    }
    private void PaperReadVoid()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 5, PaperRead))
        {

            paperToReadText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                EventManager.TeleportOutCage?.Invoke();
                ScriptableText textAssing = hit.collider.gameObject.GetComponent<PaperToRead>().textAss;
                paperToRead.GetComponent<PaperIntScript>().text = textAssing;

                paperToRead.SetActive(true);

            }

        }
        else
        {
            paperToRead.SetActive(false);
            paperToReadText.SetActive(false);
        }

    }
    private void ColonninaActivation()
    {

        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 5, colonninaActivation))
        {

            if (Instance.isWaitingForPhoto == false)
            {
                colonninaTextActivator.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (Instance.isWaitingForPhoto == false)
                    {
                        EventManager.ColonninaPhoto?.Invoke();
                        Debug.Log(Instance.isWaitingForPhoto);
                        Instance.isWaitingForPhoto = true;
                        colonninaTextActivator.SetActive(false);

                    }
                }
            }

        }
        else
        {
            colonninaTextActivator.SetActive(false);
        }



    }

    private void TeleportCube()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 50, CubeTeleport))
        {
            TeleportCubeTextActivator.SetActive(true);
            if (Input.GetKeyDown(KeyCode.T))
            {
                hit.collider.gameObject.GetComponent<CubeTeleportSelfie>().canTPhere = true;
                EventManager.TeleportInCube?.Invoke();
            }

        }
        else
            TeleportCubeTextActivator.SetActive(false);
    }

    public void OnTelePortInCage()
    {
        controller.enabled = false;
        transform.position = cageTeleport.transform.position;
        controller.enabled = true;
        PlayerMovement.Instance.hasBeenPhotoGraph = false;
        Debug.Log(PlayerMovement.Instance.hasBeenPhotoGraph);
    }
    private void RaycastCheck()
    {
        RaycastHit hit;

        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, Mathf.Infinity, ObjectMask))
            textPickUp.SetActive(true);
        else
            textPickUp.SetActive(false);


    }
    private void FinalActivation()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 5, door))
            doorText.SetActive(true);
        else
            doorText.SetActive(false);
    }
    private void ComputerText()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 20, computer))
        {
            TextComputer.SetActive(true);
        }
        else
            TextComputer.SetActive(false);
    }
    private void ElevatorActivator()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 20, Elevator))
        {
            elevatorText.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
                EventManager.ElevetorActivation?.Invoke();
        }
        else
            elevatorText.SetActive(false);
    }
    private void CameraActivator()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 20, allowPhoto))
        {
            textPickUp2.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                CanMakePhoto = true;
                Destroy(CameraToPickUp);
            }

        }
        else
            textPickUp2.SetActive(false);
    }
    private void ActivateGravity()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 5, activatorGravity))
        {
            textGravity.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.collider.gameObject.GetComponent<AnimationWallLever>().ActivateAnim();
                if (GravityInverted)
                {
                    GravityInverted = false;
                }
                else
                {
                    GravityInverted = true;
                }
            }
        }
        else
            textGravity.SetActive(false);
    }



    private void StateMachine()
    {
        // Modifica per permettere l'arrampicata solo se il giocatore guarda direttamente il muro
        if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLockAngle)
        {
            climbing = true;
            if (climbing)
            {
                StartClimbing();
                ClibingMovemnt();
            }
        }
        else
        {
            climbing = false;
            if (!climbing) StopClimbing();
        }
    }
    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadious, orientation.forward, out frontWallHit, detectonLenght, whatIsWall);

        if (wallFront)
        {
            wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);
        }

        if (isGrounded)
        {
            climbTimer = maxClimbTime;
        }
    }

    private void StartClimbing()
    {
        climbing = true;
        Debug.Log("Start Climbing");
        velocity = Vector3.zero;
    }

    private void ClibingMovemnt()
    {
        Vector3 climbDirection = Vector3.up;
        controller.Move(climbDirection * climbSpeed * Time.deltaTime);
    }

    private void StopClimbing()
    {
        climbing = false;
    }

}


