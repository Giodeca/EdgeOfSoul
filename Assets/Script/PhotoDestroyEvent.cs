using UnityEngine;

public class PhotoDestroyEvent : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.DestroyPhoto += OnDestroyEvent;
    }

    private void OnDisable()
    {
        EventManager.DestroyPhoto -= OnDestroyEvent;
    }

    private void OnDestroyEvent()
    {
        Destroy(this.gameObject);
    }
}
