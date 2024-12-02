using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionMenu;
    public GameObject buttonOption;
    public GameObject buttonMenu;
    public GameObject resumeButton;
    public static bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        PlayerMovement.Instance.CanMakePhoto = false;
        PlayerMovement.Instance.platformCount = 0;
        SceneManager.LoadScene("UiScene");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void OptionMenuAct()
    {
        optionMenu.SetActive(true);
        buttonOption.SetActive(false);
        buttonMenu.SetActive(false);
        resumeButton.SetActive(false);
    }
    public void OptionMenuActClose()
    {
        optionMenu.SetActive(false);
        buttonOption.SetActive(true);
        buttonMenu.SetActive(true);
        resumeButton.SetActive(true);
    }
}
