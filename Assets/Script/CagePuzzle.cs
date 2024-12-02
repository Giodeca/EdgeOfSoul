using System.Collections;
using UnityEngine;

public class CagePuzzle : MonoBehaviour
{
    public GameObject doorToMove;
    public GameObject door;
    public Transform endPosition;
    public Animator doorAnim;
    public GameObject leva;
    public Animator animatorLeva;
    private void OnEnable()
    {
        EventManager.TeleportOutCage += OnTelePortOutCage;
    }

    private void OnDisable()
    {
        EventManager.TeleportOutCage -= OnTelePortOutCage;
    }

    private void OnTelePortOutCage()
    {
        animatorLeva.SetTrigger("LevaAnimation");
        doorAnim.SetTrigger("OpenCage");
        //StartCoroutine(MoveDoorCage(doorToMove.transform, endPosition));
    }

    IEnumerator MoveDoorCage(Transform startingPos, Transform endPos)
    {
        float duration = 2;
        float elapseTime = 0f;

        Vector3 startingPosition = doorToMove.transform.position;

        while (elapseTime < duration)
        {
            startingPos.position = Vector3.Lerp(startingPosition, endPos.position, elapseTime / duration);
            elapseTime += Time.deltaTime;
            yield return null;
        }
        startingPos.position = endPos.position;
    }
}
