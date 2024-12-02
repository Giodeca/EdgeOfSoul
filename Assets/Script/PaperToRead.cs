using UnityEngine;

public class PaperToRead : MonoBehaviour
{
    public ScriptableText textAss;

    private void Start()
    {
        textAss = Instantiate(textAss);
    }
}
