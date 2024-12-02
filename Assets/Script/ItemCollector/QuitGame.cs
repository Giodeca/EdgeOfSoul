using UnityEngine;

public class QuitGame : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void FromQuitGame()
    {
        Debug.Log("D");
        Application.Quit();
    }
}
