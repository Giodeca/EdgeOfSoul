using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorFinalTp : MonoBehaviour
{
    public Animator anim;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StartElevator());
        }
    }

    IEnumerator StartElevator()
    {
        Debug.Log("StarCoroutio");
        yield return new WaitForSeconds(2);
        anim.SetTrigger("Open");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("TheEnd");

    }
}
