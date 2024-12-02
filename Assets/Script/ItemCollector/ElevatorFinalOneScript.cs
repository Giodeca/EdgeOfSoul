using UnityEngine;

public class ElevatorFinalOneScript : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float movementSpeed = 0.5f;
    public GameObject player;

    void Start()
    {
        player.transform.position = endPos.position;
    }

    void Update()
    {


    }




}
