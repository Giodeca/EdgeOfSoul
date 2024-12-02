using UnityEngine;


[System.Serializable]
public struct CustomRotation
{
    public float x;
    public float y;
    public float z;

    //public Quaternion ToQuaternion()
    //{
    //    return Quaternion.Euler(x, y, z);
    //}
}
public class DecalScript : MonoBehaviour
{
    public GameObject playerParent;
    public Rigidbody body;
    public GameObject quad;
    public GameObject Decal;
    public MeshRenderer mesh;
    [SerializeField] private CustomRotation rotation = new CustomRotation();
    public bool hasBennPicked;
    [SerializeField] private MeshCollider meshCollider;


    //public CustomRotation Rotation
    //{
    //    get { return rotation; }
    //    set { rotation = value; }
    //}
    private Transform previousParent;

    private void Start()
    {
        previousParent = transform.parent;

    }

    private void Update()
    {

        if (transform.parent != previousParent)
        {
            quad.SetActive(false);
            Decal.SetActive(false);
            mesh.enabled = true;
            //transform.rotation = rotation.ToQuaternion();
            body.useGravity = true;
            meshCollider.isTrigger = false;
        }
        //else
        //{
        //    quad.SetActive(true);
        //    Decal.SetActive(true);
        //    mesh.enabled = false;
        //    hasBennPicked = true;
        //    transform.rotation = Quaternion.Euler(0, 0, 0);
        //    body.useGravity = false;
        //}
    }


}
