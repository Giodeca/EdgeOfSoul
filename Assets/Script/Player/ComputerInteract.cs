using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    public LayerMask computer;
    public GameObject PCPanel;
    public GameObject text;
    //public ActivateComputer PCScript;
    private void Update()
    {
        ActivateComputer();
    }

    private void ActivateComputer()
    {
        if (Physics.Raycast(transform.position, transform.forward, 5f, computer))
        {
            if (PlayerMovement.Instance.canUsePC == true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("clicked E");
                    PlayerMovement.Instance.StopRotatiom = true;
                    PCPanel.gameObject.SetActive(true);
                    text.SetActive(false);
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
        else
        {
            if (PCPanel != null)
                PCPanel.gameObject.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            PlayerMovement.Instance.StopRotatiom = false;


        }
    }
}
