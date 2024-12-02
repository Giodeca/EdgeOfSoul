using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(MeshFilter))]
public class SliceThings : MonoBehaviour
{
    // Variabile statica per il materiale predefinito
    static Material DefaultMaterial;

    // Nome dello shader utilizzato per creare il materiale predefinito
    static readonly string ShaderString = "Universal Render PipeLine/Lit";

    // Materiale per il taglio, serializzato per essere modificato nell'editor di Unity
    [SerializeField] Material CuttingMaterial = null;

    // Proprietà booleana che indica se l'istanza corrente dell'oggetto è una copia o meno
    public bool isCopy { get; private set; } = false;

    // Metodo statico chiamato una volta quando viene caricata la scena
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        // Se il materiale predefinito è già stato inizializzato, esce dal metodo
        if (DefaultMaterial) return;

        // Verifica se lo shader specificato è presente nel progetto di Unity
        if (!Shader.Find(ShaderString))
        {
            // Stampa un errore nel log di Unity se lo shader non è stato trovato
            UnityEngine.Debug.LogError($"Shader {ShaderString} not found.");
            return;
        }

        // Crea un nuovo materiale utilizzando lo shader trovato precedentemente
        DefaultMaterial = new Material(Shader.Find(ShaderString))
        {
            // Imposta il colore del materiale su grigio
            color = Color.gray
        };

        // Imposta la rugosità (smoothness) a 0
        DefaultMaterial.SetInt("_Smoothness", 0);
    }

    // Metodo chiamato quando l'oggetto su cui è attaccato questo script viene inizializzato
    private void Start()
    {
        // Controlla se il materiale di taglio non è stato assegnato o se lo shader è nullo
        if (!CuttingMaterial || !CuttingMaterial.shader)
            // Se è così, imposta il materiale di taglio sul materiale predefinito
            CuttingMaterial = DefaultMaterial;
    }

    // Metodo pubblico che imposta l'oggetto corrente come una copia
    public void SetAsCopy()
    {
        // Se l'oggetto è già una copia, esce dal metodo
        if (isCopy)
            return;

        // Imposta isCopy su true
        isCopy = true;

        // Ottiene il componente Renderer dell'oggetto
        var render = GetComponent<Renderer>();

        // Aggiorna i materiali del renderer includendo il materiale originale e il materiale di taglio
        render.materials = new Material[]
        {
            render.material,
            CuttingMaterial
        };
    }
}

