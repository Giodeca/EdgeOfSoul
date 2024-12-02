using UnityEngine;

public class ElevatorBirthDay : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public bool IsMoving;
    public float movementSpeed = 0.5f;
    private Vector3 startPosition;
    private Vector3 endPosition;

    private void OnEnable()
    {
        EventManager.ElevetorOn += OnElevatorOn;
        EventManager.ElevetorOff += OnElevatorOff;
    }
    private void OnDisable()
    {
        EventManager.ElevetorOn -= OnElevatorOn;
        EventManager.ElevetorOff -= OnElevatorOff;
    }
    void Start()
    {
        startPosition = startPos.position;
        endPosition = endPos.position;
    }

    void Update()
    {
        if (IsMoving)
        {
            IsMovingElevator();
        }
    }

    void IsMovingElevator()
    {

        float time = Mathf.PingPong(Time.time * movementSpeed, 1f);
        transform.position = Vector3.Lerp(startPosition, endPosition, time);
    }

    private void OnElevatorOn()
    {
        IsMoving = true;
    }
    private void OnElevatorOff()
    {
        IsMoving = false;
    }


}
