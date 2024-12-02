using UnityEngine;

public class DestoruCube : MonoBehaviour
{

    private void OnEnable()
    {
        EventManager.DestroyCube += OnDestroyCube;
    }

    private void OnDisable()
    {
        EventManager.DestroyCube -= OnDestroyCube;
    }
    private void OnDestroyCube()
    {
        Destroy(gameObject);
    }
}
