
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public Vector3 respawnPos;
    public Vector3 ResetPos;

    protected override void Awake()
    {
        base.Awake();
        EventManager.SavePosition += OnSavePosition;
        ResetPos = respawnPos;
    }


    private void OnDisable()
    {
        EventManager.SavePosition -= OnSavePosition;
    }
    private void OnSavePosition(Vector3 position)
    {
        respawnPos = position;
        Debug.Log(respawnPos);
    }

    public void RestartGamePos()
    {
        respawnPos = ResetPos;
        PauseMenu.isPaused = false;
    }
}
