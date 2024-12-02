using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public GameObject panelOption;
    public GameObject panelCredits;
    public void StartGame()
    {
        SceneManager.LoadScene("Final Scene");
        SpawnManager.Instance.RestartGamePos();
    }

    public void Continue()
    {
        SceneManager.LoadScene("Final Scene");
    }
    public void Option()
    {
        panelOption.SetActive(true);
    }
    public void Credits()
    {
        panelCredits.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void BackCredits()
    {
        panelCredits.SetActive(false);
    }
    public void BackOption()
    {
        panelOption.SetActive(false);
    }
}
