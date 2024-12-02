using UnityEngine;

public class CubeGravityInverted : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GravityInverted();
    }
    void GravityInverted()
    {
        // Inverte la gravità per questo rigidbody
        rb.useGravity = false;
        rb.AddForce(Vector3.up * Physics.gravity.magnitude, ForceMode.Acceleration);
    }
}
