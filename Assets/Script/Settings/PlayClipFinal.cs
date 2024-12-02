using UnityEngine;

public class PlayClipFinal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlayMusic(AudioManager.Instance.EndGame);
            EventManager.EndIsNear?.Invoke();
        }

    }
}
