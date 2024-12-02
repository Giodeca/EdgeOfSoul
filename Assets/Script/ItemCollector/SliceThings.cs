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

    // Propriet� booleana che indica se l'istanza corrente dell'oggetto � una copia o meno
    public bool isCopy { get; private set; } = false;

    // Metodo statico chiamato una volta quando viene caricata la scena
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        // Se il materiale predefinito � gi� stato inizializzato, esce dal metodo
        if (DefaultMaterial) return;

        // Verifica se lo shader specificato � presente nel progetto di Unity
        if (!Shader.Find(ShaderString))
        {
            // Stampa un errore nel log di Unity se lo shader non � stato trovato
            UnityEngine.Debug.LogError($"Shader {ShaderString} not found.");
            return;
        }

        // Crea un nuovo materiale utilizzando lo shader trovato precedentemente
        DefaultMaterial = new Material(Shader.Find(ShaderString))
        {
            // Imposta il colore del materiale su grigio
            color = Color.gray
        };

        // Imposta la rugosit� (smoothness) a 0
        DefaultMaterial.SetInt("_Smoothness", 0);
    }

    // Metodo chiamato quando l'oggetto su cui � attaccato questo script viene inizializzato
    private void Start()
    {
        // Controlla se il materiale di taglio non � stato assegnato o se lo shader � nullo
        if (!CuttingMaterial || !CuttingMaterial.shader)
            // Se � cos�, imposta il materiale di taglio sul materiale predefinito
            CuttingMaterial = DefaultMaterial;
    }

    // Metodo pubblico che imposta l'oggetto corrente come una copia
    public void SetAsCopy()
    {
        // Se l'oggetto � gi� una copia, esce dal metodo
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

