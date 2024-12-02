using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabObject2 : MonoBehaviour
{
    [SerializeField]
    private Transform _cameraTransform;

    [SerializeField]
    private LayerMask _pickupableLayer;

    [SerializeField]
    private LayerMask _heldObjectLayer;
    [SerializeField]
    private LayerMask _wallLayers;
    private Transform _heldObject;
    private Rigidbody _heldObjectsRb;

    private Vector3 _left;
    private Vector3 _right;
    private Vector3 _top;
    private Vector3 _bottom;
    private float _orgDInstanceToScaleRatio;
    private Vector3 _orgViewportPos;

    private List<Vector3> _shapedGrid = new List<Vector3>();

    [SerializeField]
    private int NUMBER_OF_GRID_ROWS = 10;
    [SerializeField]
    private int NUMBER_OF_GRID_COLUMNS = 10;
    private const float SCALE_MARGIN = .001f;

    [SerializeField] private Image _image;
    [SerializeField] private Sprite _spriteOpen;
    [SerializeField] private Sprite _spriteClose;

    [SerializeField]
    private LayerMask _DuplicateLayer;

    private bool isPickable;
    private bool isDuplicate;


    private void Awake()
    {
        EventManager.PickUpObject += OnPickup;
    }
    private void OnDisable()
    {
        EventManager.PickUpObject -= OnPickup;
    }
    private void FixedUpdate()
    {
        if (_heldObject == null) return;

        MoveInFrontOfObstacles();

        UpdateScale();
    }
    private void MoveInFrontOfObstacles()
    {
        if (_shapedGrid.Count == 0) throw new System.Exception("Shaped grid calculation error");

        float closestZ = 1000;
        for (int i = 0; i < _shapedGrid.Count; i++)
        {
            RaycastHit hit = CastTowardsGridPoint(_shapedGrid[i], _wallLayers + _pickupableLayer);
            if (hit.collider == null) continue;

            Vector3 wallPoint = _cameraTransform.InverseTransformPoint(hit.point);
            if (i == 0 || wallPoint.z < closestZ)
            {

                closestZ = wallPoint.z;
            }
        }


        float boundsMagnitude = _heldObject.GetComponent<Renderer>().localBounds.extents.magnitude * _heldObject.localScale.x;
        Vector3 newLocalPos = _heldObject.localPosition;
        newLocalPos.z = closestZ - boundsMagnitude;
        _heldObject.localPosition = newLocalPos;
    }
    private void UpdateScale()
    {
        float newScale = (_cameraTransform.position - _heldObject.position).magnitude / _orgDInstanceToScaleRatio;
        if (Mathf.Abs(newScale - _heldObject.localScale.x) < SCALE_MARGIN) return;

        _heldObject.localScale = new Vector3(newScale, newScale, newScale);

        Vector3 newPos = Camera.main.ViewportToWorldPoint(new Vector3(_orgViewportPos.x, _orgViewportPos.y,
            (_heldObject.position - _cameraTransform.position).magnitude));
        _heldObject.position = newPos;
    }
    private void OnDrawGizmos()
    {
        if (_heldObject == null) return;

        //Hits
        Gizmos.matrix = _cameraTransform.localToWorldMatrix;
        Gizmos.color = Color.green;
        foreach (Vector3 point in _shapedGrid)
        {
            Gizmos.DrawSphere(point, .01f);
        }
    }
    public void OnPickup()
    {
        if (_heldObject != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.PickUpSound2);
            _heldObject.parent = null;
            _heldObjectsRb.useGravity = true;
            _heldObjectsRb.constraints = RigidbodyConstraints.None;
            if (isPickable)
                _heldObject.gameObject.layer = (int)Mathf.Log(_pickupableLayer.value, 2);
            else if (isDuplicate)
                _heldObject.gameObject.layer = (int)Mathf.Log(_DuplicateLayer.value, 2);
            _heldObject = null;
            _image.sprite = _spriteOpen;
            return;
        }


        RaycastHit hit2;
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit2, 100, _pickupableLayer))
        {
            _image.sprite = _spriteClose;
        }
        else
            _image.sprite = _spriteOpen;

        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, 100, _pickupableLayer))
        {
            RaycastHit hit;
            Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, 100, _pickupableLayer);

            if (hit.collider == null) return;

            AudioManager.Instance.PlaySFX(AudioManager.Instance.PickUpSound);
            _heldObject = hit.collider.gameObject.transform;
            _heldObjectsRb = _heldObject.GetComponent<Rigidbody>();

            float scale = _heldObject.localScale.x;
            if (Mathf.Abs(scale - _heldObject.localScale.y) > SCALE_MARGIN
                || Mathf.Abs(scale - _heldObject.localScale.z) > SCALE_MARGIN)
                throw new System.Exception("Wrong Pickupable object's scale!");
            _orgDInstanceToScaleRatio = (_cameraTransform.position - _heldObject.position).magnitude / scale;

            _heldObject.gameObject.layer = (int)Mathf.Log(_heldObjectLayer.value, 2);
            _heldObjectsRb.useGravity = false;
            _heldObject.parent = _cameraTransform;
            // converte  è un metodo che converte le coordinate di un punto nello spazio del mondo in
            // coordinate di visualizzazione relative alla telecamera specificata.
            // determinare la posizione di un oggetto nella vista della telecamera in un dato momento.
            _orgViewportPos = Camera.main.WorldToViewportPoint(_heldObject.position);
            _heldObjectsRb.constraints = RigidbodyConstraints.FreezeAll;

            Vector3[] bbPoints = GetBoundingBoxPoints();
            SetupShapedGrid(bbPoints);
            isPickable = true;
            isDuplicate = false;
        }

        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, 100, _DuplicateLayer))
            DuplicateObject();
    }


    private void DuplicateObject()
    {

        RaycastHit hit;
        Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, 100, _DuplicateLayer);


        if (hit.collider == null) return;
        _heldObject = hit.collider.gameObject.transform;
        GameObject copy = hit.collider.gameObject;
        GameObject copyOfCube = Instantiate(copy, hit.collider.gameObject.transform.position, Quaternion.identity);
        _heldObject = copyOfCube.transform;
        _heldObjectsRb = copyOfCube.GetComponent<Rigidbody>();

        float scale = _heldObject.localScale.x;
        if (Mathf.Abs(scale - _heldObject.localScale.y) > SCALE_MARGIN
            || Mathf.Abs(scale - _heldObject.localScale.z) > SCALE_MARGIN)
            throw new System.Exception("Wrong Pickupable object's scale!");
        _orgDInstanceToScaleRatio = (_cameraTransform.position - _heldObject.position).magnitude / scale;

        _heldObject.gameObject.layer = (int)Mathf.Log(_heldObjectLayer.value, 2);
        _heldObjectsRb.useGravity = false;
        _heldObject.transform.parent = _cameraTransform;
        // converte  è un metodo che converte le coordinate di un punto nello spazio del mondo in
        // coordinate di visualizzazione relative alla telecamera specificata.
        // determinare la posizione di un oggetto nella vista della telecamera in un dato momento.
        _orgViewportPos = Camera.main.WorldToViewportPoint(_heldObject.position);
        _heldObjectsRb.constraints = RigidbodyConstraints.FreezeAll;

        Vector3[] bbPoints = GetBoundingBoxPoints();
        SetupShapedGrid(bbPoints);
        isPickable = false;
        isDuplicate = true;
    }
    #region Calculating grid
    private Vector3[] GetBoundingBoxPoints()
    {
        // Ottieni le dimensioni del bounding box locale dell'oggetto
        Vector3 size = _heldObject.GetComponent<Renderer>().localBounds.size;

        // Definisci vettori per gli assi x, y e z
        Vector3 x = new Vector3(size.x, 0, 0);
        Vector3 y = new Vector3(0, size.y, 0);
        Vector3 z = new Vector3(0, 0, size.z);

        // Ottieni il punto minimo del bounding box locale dell'oggetto
        Vector3 min = _heldObject.GetComponent<Renderer>().localBounds.min;

        // Calcola i punti del bounding box
        Vector3[] bbPoints =
        {
        min,
        min + x,
        min + y,
        min + x + y,
        min + z,
        min + z + x,
        min + z + y,
        min + z + x + y
    };

        return bbPoints;
    }

    private void SetupShapedGrid(Vector3[] bbPoints)
    {
        // Inizializza le variabili che definiscono i limiti della griglia
        _left = _right = _top = _bottom = Vector2.zero;

        // Calcola i limiti della griglia basati sui punti del bounding box
        GetRectConfines(bbPoints);

        // Configura la griglia basata sulla forma dell'oggetto
        Vector3[,] grid = SetupGrid();
        GetShapedGrid(grid);
    }

    private void GetRectConfines(Vector3[] bbPoints)
    {
        Vector3 bbPoint; // Punto del bounding box
        Vector3 cameraPoint; // Punto della telecamera
        Vector2 viewportPoint; // Coordinate di visualizzazione

        // Calcola il punto più vicino dell'oggetto alla telecamera
        Vector3 closestPoint = _heldObject.GetComponent<Renderer>().localBounds.ClosestPoint(_cameraTransform.position);
        // Calcola la coordinata z del punto più vicino rispetto alla telecamera
        float closestZ = _cameraTransform.InverseTransformPoint(_heldObject.TransformPoint(closestPoint)).z;
        // Controlla che il punto più vicino sia davanti alla telecamera
        if (closestZ <= 0) throw new System.Exception("HeldObject's inside the player!");

        // Loop attraverso tutti i punti del bounding box
        for (int i = 0; i < bbPoints.Length; i++)
        {
            // Trasforma il punto del bounding box nello spazio globale le Illusioni
            bbPoint = _heldObject.TransformPoint(bbPoints[i]);
            // Converti il punto nello spazio globale in coordinate di visualizzazione relative alla telecamera
            viewportPoint = Camera.main.WorldToViewportPoint(bbPoint);
            // Trasforma il punto nello spazio globale in coordinate locali della telecamera
            cameraPoint = _cameraTransform.InverseTransformPoint(bbPoint);
            // Imposta la coordinata z del punto della telecamera sul valore calcolato precedentemente
            cameraPoint.z = closestZ;

            // Se il punto è fuori dalla vista della telecamera, salta al prossimo punto
            if (viewportPoint.x < 0 || viewportPoint.x > 1
                || viewportPoint.y < 0 || viewportPoint.y > 1) continue;

            // Se è il primo punto, inizializza i limiti
            if (i == 0) _left = _right = _top = _bottom = cameraPoint;

            // Aggiorna i limiti in base alla posizione del punto
            if (cameraPoint.x < _left.x) _left = cameraPoint;
            if (cameraPoint.x > _right.x) _right = cameraPoint;
            if (cameraPoint.y > _top.y) _top = cameraPoint;
            if (cameraPoint.y < _bottom.y) _bottom = cameraPoint;
        }
    }

    private Vector3[,] SetupGrid()
    {
        // Calcola la lunghezza orizzontale e verticale del rettangolo
        float rectHrLength = _right.x - _left.x;
        float rectVertLength = _top.y - _bottom.y;

        // Calcola l'incremento orizzontale e verticale tra i punti della griglia
        Vector3 hrStep = new Vector2(rectHrLength / (NUMBER_OF_GRID_COLUMNS - 1), 0);
        Vector3 vertStep = new Vector2(0, rectVertLength / (NUMBER_OF_GRID_ROWS - 1));

        // Crea un array bidimensionale per memorizzare i punti della griglia
        Vector3[,] grid = new Vector3[NUMBER_OF_GRID_ROWS, NUMBER_OF_GRID_COLUMNS];

        // Imposta il primo punto della griglia con le coordinate del limite inferiore sinistro del rettangolo
        grid[0, 0] = new Vector3(_left.x, _bottom.y, _left.z);

        // Loop attraverso ogni riga e colonna della griglia
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int w = 0; w < grid.GetLength(1); w++)
            {
                // Se è il primo punto, salta al prossimo senza fare nulla
                if (i == 0 && w == 0) continue;
                // Se è il primo punto di una nuova riga, imposta il punto come punto della riga precedente più lo spostamento verticale
                else if (w == 0)
                {
                    grid[i, w] = grid[i - 1, 0] + vertStep;
                }
                // Altrimenti, imposta il punto come punto della stessa riga precedente più lo spostamento orizzontale
                else
                {
                    grid[i, w] = grid[i, w - 1] + hrStep;
                }
            }
        }

        // Restituisce l'array bidimensionale contenente i punti della griglia
        return grid;
    }
    private void GetShapedGrid(Vector3[,] grid)
    {
        // Pulisce la griglia formata
        _shapedGrid.Clear();

        // Scansiona ogni punto nella griglia
        foreach (Vector3 point in grid)
        {
            // Lancia un raggio verso il punto nella griglia
            RaycastHit hit = CastTowardsGridPoint(point, _heldObjectLayer);

            // Se il raggio colpisce un collider
            if (hit.collider != null)
            {
                // Aggiunge il punto alla griglia formata
                _shapedGrid.Add(point);
            }
        }
    }
    #endregion

    // Lancia un raggio dal punto sulla griglia verso il basso per rilevare i collider
    private RaycastHit CastTowardsGridPoint(Vector3 gridPoint, LayerMask layers)
    {
        // Trasforma il punto della griglia nello spazio mondiale
        Vector3 worldPoint = _cameraTransform.TransformPoint(gridPoint);

        // Converte il punto mondiale in coordinate di viewport
        Vector3 origin = Camera.main.WorldToViewportPoint(worldPoint);
        origin.z = 0;
        origin = Camera.main.ViewportToWorldPoint(origin);

        // Calcola la direzione del raggio dal punto di vista della telecamera al punto sulla griglia
        Vector3 direction = worldPoint - origin;

        // Lancia un raggio dalla posizione del punto verso il basso e rileva i collider
        RaycastHit hit;
        Physics.Raycast(origin, direction, out hit, 1000, layers);

        // Restituisce l'informazione sul raggio
        return hit;
    }
}
