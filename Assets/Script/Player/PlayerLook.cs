using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] Transform playerBody;
    float xRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (PlayerMovement.Instance.isUsingPC == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (PlayerMovement.Instance.StopRotatiom == false)
                {
                    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                    xRotation -= mouseY;
                    xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                    transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
                    playerBody.Rotate(Vector3.up * mouseX);
                }
            }
            else if (PlayerMovement.Instance.isUsingPC == true)
            {
                Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
            }

        }
        else
        {
            PlayerMovement.Instance.isUsingPC = false;
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        }


    }
}
