using System.Collections;
using UnityEngine;

public class Vault : MonoBehaviour
{
    private int vaultLayer;
    public Camera cam;
    public float playerHeight = 5f;
    public float playerRadius = 0.6f;

    void Start()
    {
        vaultLayer = LayerMask.NameToLayer("VaultLayer");
        vaultLayer = ~vaultLayer;
    }
    void Update()
    {
        if (!PauseMenu.isPaused)
            VaultAction();
    }





    private void VaultAction()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var firstHit, 1f, vaultLayer))
            {
                print("vaultable in front");
                if (Physics.Raycast(firstHit.point + (cam.transform.forward * playerRadius) + (Vector3.up * 0.6f * playerHeight), Vector3.down, out var secondHit, playerHeight))
                {
                    print("found place to land");
                    StartCoroutine(LerpVault(secondHit.point, 0.5f));
                }
            }
        }

    }
    IEnumerator LerpVault(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }


}
