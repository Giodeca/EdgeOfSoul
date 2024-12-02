using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorFinal : MonoBehaviour
{
    //public ElevatorFinalOneScript elev;
    public Animator animElev;
    public GameObject player;
    public Transform endPos;
    private void OnEnable()
    {
        EventManager.FinalElveatorActivationEventOn += FinalElevatorActivation;
    }

    private void OnDisable()
    {
        EventManager.FinalElveatorActivationEventOn -= FinalElevatorActivation;
    }

    private void FinalElevatorActivation()
    {
        StartCoroutine(StartElevator());

    }


    IEnumerator StartElevator()
    {
        Debug.Log("StarCoroutio");
        animElev.SetTrigger("Open");
        yield return new WaitForSeconds(2);
        animElev.SetTrigger("Close");
        yield return new WaitForSeconds(1);

        if (PlayerMovement.Instance.isInFinalPlace == true)
            SceneManager.LoadScene("TheEnd");


    }
}
