using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject ElevatorToMove;
    public Transform endPosition;
    private void OnEnable()
    {
        EventManager.ElevetorActivation += OnElevetator;
    }

    private void OnDisable()
    {
        EventManager.ElevetorActivation -= OnElevetator;
    }

    private void OnElevetator()
    {
        StartCoroutine(MoveDoorCage(ElevatorToMove.transform, endPosition));
    }

    IEnumerator MoveDoorCage(Transform startingPos, Transform endPos)
    {
        float duration = 2;
        float elapseTime = 0f;

        Vector3 startingPosition = ElevatorToMove.transform.position;

        while (elapseTime < duration)
        {
            startingPos.position = Vector3.Lerp(startingPosition, endPos.position, elapseTime / duration);
            elapseTime += Time.deltaTime;
            yield return null;
        }
        startingPos.position = endPos.position;
    }
}
