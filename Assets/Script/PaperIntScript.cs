using TMPro;
using UnityEngine;

public class PaperIntScript : MonoBehaviour
{
    public TMP_Text textToChange;
    public ScriptableText text;
    public GameObject paper;

    private void OnEnable()
    {
        textToChange.text = text.text;
    }



    public void ClosePaper()
    {
        paper.SetActive(false);
    }
}
