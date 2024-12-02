using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiScripts : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text UIText;
    public GameObject BottomLine;
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIText.color = Color.red;

        if (BottomLine != null)
            BottomLine.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIText.color = Color.white;
        if (BottomLine != null)
            BottomLine.SetActive(false);
    }
}
