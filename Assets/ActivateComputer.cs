using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivateComputer : MonoBehaviour
{
    public GameObject panelPC;
    public GameObject LoginGroup;
    public GameObject MainScreenGroup;
    public RotateDoor Door1;
    public RotateDoor Door2;

    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;


    [Header("visual section")]
    public Sprite unlockedLock;

    public Image lockImage;
    public Transform player;
    public string username = "bob";
    public string password = "123456";

    public string keyPassword = "4725";

    private string inputtedUsername;
    private string inputtedPassword;
    private string inputtedKeyPassword;
    public GameObject KeyLoginGroup;

    public TMP_Text text;
    public TMP_Text TextDoorOpen;
    public TMP_Text WrongUsernamePasswordText;
    public TMP_Text WrongKeyPassword;
        

    private void Start()
    {
        LoginGroup.SetActive(false);
        MainScreenGroup.SetActive(false);
        KeyLoginGroup.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerMovement.Instance.isUsingPC = true;
        Debug.Log(PlayerMovement.Instance.isUsingPC);
        StartCoroutine(StartLoginSequence());
    }
    private void OnDisable()
    {

    }

    public IEnumerator TextCoroutine()
    {
        text.text = "Loading.";
        yield return new WaitForSeconds(0.4f);
        text.text = "Loading..";
        yield return new WaitForSeconds(0.4f);
        text.text = "Loading...";
        yield return new WaitForSeconds(0.4f);
        text.text = "Loading.";
        yield return new WaitForSeconds(0.4f);
        text.text = "Loading..";
        yield return new WaitForSeconds(0.4f);
        text.text = "Loading...";
    }
    public IEnumerator StartLoginSequence()
    {
        StartCoroutine(TextCoroutine());
        yield return new WaitForSeconds(2f);
        text.enabled = false;
        LoginGroup.SetActive(true);

    }

    public void CathUsername(string userInput)
    {
        inputtedUsername = userInput;
        Debug.Log(inputtedUsername);
    }

    public void CatchUserPassword(string userInput)
    {
        inputtedPassword = userInput;
        Debug.Log(inputtedPassword);
    }

    public void ConvalidateAccess()
    {
        Debug.Log("entered button");
        if (inputtedPassword == password && inputtedUsername == username)
        {
            Debug.Log("entered");
            MainScreenGroup.SetActive(true);
            LoginGroup.SetActive(false);
        }
        else
        {
            StartCoroutine(WrongTextFeedBack(WrongUsernamePasswordText));
        }
    }

    public void KeyLogin()
    {
        KeyLoginGroup.SetActive(true);
    }

    public void CatchUserKey(string userInput)
    {
        inputtedKeyPassword = userInput;
    }

    public void ButtonKey()
    {
        if (inputtedKeyPassword == keyPassword)
        {
            Door1.Open(player.transform.position);
            Door2.Open(player.transform.position);
            lockImage.sprite = unlockedLock;
            StartCoroutine(FeedBackText());
        }
        else
        {
            StartCoroutine(WrongTextFeedBack(WrongKeyPassword));
        }
    }
    IEnumerator FeedBackText()
    {
        TextDoorOpen.enabled = true;
        yield return new WaitForSeconds(2f);
        TextDoorOpen.enabled = false;
    }
    public void ExitFromScreen()
    {
        panelPC.SetActive(false);
        PlayerMovement.Instance.StopRotatiom = false;
        PlayerMovement.Instance.isUsingPC = false;
        inputtedPassword = string.Empty;

        usernameInput.text = "";
        inputtedUsername = string.Empty;

        passwordInput.text = "";

    }

    IEnumerator WrongTextFeedBack(TMP_Text objectToActivate)
    {
        objectToActivate.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        objectToActivate.gameObject.SetActive(false);
    }
}

