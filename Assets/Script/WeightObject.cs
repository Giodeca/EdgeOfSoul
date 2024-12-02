using UnityEngine;

public class WeightObject : MonoBehaviour
{
    public float weight;
    public GameObject scale;


    private void OnEnable()
    {
        EventManager.EndIsNear += DestoyCubeAtThe;
    }


    private void OnDisable()
    {
        EventManager.EndIsNear -= DestoyCubeAtThe;
    }


    private void DestoyCubeAtThe()
    {
        Destroy(this.gameObject);
    }
    private void Update()
    {
        weight = scale.transform.localScale.x;
    }
}
