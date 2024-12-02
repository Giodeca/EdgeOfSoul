using System.Collections;
using UnityEngine;

public class ClosingDoor : MonoBehaviour
{

    [SerializeField]
    private RotateDoor Door;

    public bool isOpenDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpenDoor)
            return;
        //if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        //{
        //    StopCoroutine(AutomaticClosingDoor());
        //}
    }


    private void OnTriggerExit(Collider other)
    {
        if (isOpenDoor)
        {
            return;
        }
        //if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        //{
        //    StartCoroutine(AutomaticClosingDoor());
        //}
    }



    //IEnumerator AutomaticClosingDoor()
    //{
    //    yield return new WaitForSeconds(3);
    //    if (Door.IsOpen)
    //    {

    //        Door.Close();
    //    }
    //}
}
