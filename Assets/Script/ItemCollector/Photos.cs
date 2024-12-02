using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Photos : ItemCollector
{
    // These fields are serialized, meaning they can be set in the Unity Editor
    /*   [SerializeField] Camera CameraBackground; */// Reference to the camera used for background
    [SerializeField] Camera CameraObjects;    // Reference to the camera capturing objects
    [SerializeField] Camera CameraMain;
    // This GameObject will hold the output of the photographed objects
    GameObject PhotoOutputParent;

    // Texture to hold the picture of the objects
    public Texture PictureTexture;

    // List to store the objects to be projected
    List<SliceThings> projections;
    List<SliceThings> ThingsProjections;

    // Array of Planes to represent the frustum planes of the object camera
    Plane[] planes;

    // Texture to hold the background image
    Texture BackgroundTexture;

    // Static Material and Mesh for the background
    static Material BackgroundMaterial;
    static Mesh BackgroundMesh;

    // This method is called when the script instance is being loaded
    void Awake()
    {
        // Initialize the array of Planes
        planes = new Plane[6];

        // Check if the BackgroundMaterial is null
        if (BackgroundMaterial is null)
        {
            // Create a new material for the background with a shader
            BackgroundMaterial = new Material(Shader.Find("Unlit/Texture"))
            {
                // Set the main texture to the background texture and color to white
                mainTexture = BackgroundTexture,
                color = Color.white
            };
            // Set the smoothness to 0
            BackgroundMaterial.SetInt("_Smoothness", 0);
        }

        // If BackgroundMesh is already set, return
        if (BackgroundMesh)
            return;

        // Calculate the dimensions of the background mesh based on camera parameters
        var dInstance = CameraObjects.farClipPlane / 4;
        var length = Mathf.Tan(CameraObjects.fieldOfView * Mathf.Deg2Rad / 2) * dInstance;

        // Create a new mesh for the background with specified vertices, triangles, and UVs
        BackgroundMesh = new Mesh
        {
            vertices = new Vector3[]
            {
            new Vector3(-length, length, dInstance),
            new Vector3(length, length, dInstance),
            new Vector3(length, -length, dInstance),
            new Vector3(-length, -length, dInstance),
            },
            triangles = new[] { 0, 1, 3, 1, 2, 3 },
            uv = new[]
            {
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0),
        }
        };
    }

    // This method captures the photograph of the objects
    public void PhotoGraph()
    {
        // Calculate frustum planes for the object camera
        GeometryUtility.CalculateFrustumPlanes(CameraObjects, planes);

        // Initialize the list of projected objects
        projections = new List<SliceThings>();

        // Find all SliceThings objects that are active and enabled in the scene
        SliceThings[] Projections = FindObjectsOfType<SliceThings>().Where(p => p.isActiveAndEnabled).ToArray();

        // Capture the background texture and picture texture
        //BackgroundTexture = TextureScrpt.Screenshot(CameraBackground);
        PictureTexture = TextureScrpt.Screenshot(CameraObjects);

        // Set the main texture of the renderer to the picture texture
        GetComponent<Renderer>().material.mainTexture = PictureTexture;

        // Iterate through each projection object
        foreach (var projection in Projections)
        {
            // If the projection object is not active, skip it
            if (!projection.gameObject.activeInHierarchy)
                continue;

            // Try to get the renderer component of the projection object
            projection.TryGetComponent<Renderer>(out var renderer);

            // If no renderer is found, skip it
            if (!renderer)
                continue;

            // Get the bounding box of the renderer
            var bounds = renderer.bounds;

            // If the bounding box is within the frustum planes, add it to the list of projections
            if (GeometryUtility.TestPlanesAABB(planes, bounds))
                projections.Add(projection);
        }

        // Set the current GameObject to active
        gameObject.SetActive(true);

        // Copy the projected objects to the photo output
        CopyObjects();

        // Set up the background image
        // Add a MeshRenderer component to the PhotoOutputParent and set its material and mesh
        var rend = PhotoOutputParent.AddComponent<MeshRenderer>();
        rend.material = BackgroundMaterial;
        rend.material.mainTexture = BackgroundTexture;

        //PhotoOutputParent.AddComponent<MeshFilter>().sharedMesh = BackgroundMesh;

    }


    // This method copies the objects to the photo output
    public void CopyObjects()
    {
        // Create a new GameObject to hold the photo output
        PhotoOutputParent = new GameObject("Photo Output");
        PhotoOutputParent.transform.position = CameraObjects.transform.position;
        PhotoOutputParent.transform.rotation = CameraObjects.transform.rotation;

        // Iterate through each original object to be projected
        foreach (var original in projections)
        {
            // Instantiate a copy of the original object under the PhotoOutputParent
            var copy = Instantiate(
                    original,
                    PhotoOutputParent.transform,
                    true
                );

            // Get the renderer components of the copy and original objects
            var renderCopy = copy.GetComponent<Renderer>();
            var renderOriginal = original.GetComponent<Renderer>();

            copy.SetAsCopy();
            //Debug.Log(copy.name);

            // Copy lightmap settings from the original to the copy
            renderCopy.lightmapIndex = renderOriginal.lightmapIndex;
            renderCopy.lightmapScaleOffset = renderOriginal.lightmapScaleOffset;

            // Cut the copy mesh by the frustum planes
            MeshCut.CutByPlanes(copy, planes);

            // If the copy mesh has no vertices, destroy it
            if (copy.GetComponent<MeshFilter>()?.mesh.vertices.Length == 0)
                Destroy(copy);
        }
        // Set the PhotoOutputParent to inactive
        PhotoOutputParent.SetActive(false);
    }

    //public void CopyObjectsToDestroy()
    //{
    //    // Iterate through each original object to be projected
    //    foreach (var original in projections)
    //    {
    //        // Instantiate a copy of the original object under the PhotoOutputParent
    //        var copy = Instantiate(
    //                original,
    //                PhotoOutputParent.transform,
    //                true
    //            );

    //        // Get the renderer components of the copy and original objects
    //        var renderCopy = copy.GetComponent<Renderer>();
    //        var renderOriginal = original.GetComponent<Renderer>();

    //        // Copy lightmap settings from the original to the copy
    //        renderCopy.lightmapIndex = renderOriginal.lightmapIndex;
    //        renderCopy.lightmapScaleOffset = renderOriginal.lightmapScaleOffset;

    //        // Cut the copy mesh by the frustum planes
    //        MeshCut.CutByPlanes(copy, planes);

    //        // If the copy mesh has no vertices, destroy it
    //        if (copy.GetComponent<MeshFilter>()?.mesh.vertices.Length == 0)
    //            Destroy(copy.gameObject); // Instead of destroying the mesh, destroy the whole GameObject
    //    }
    //}


    public void DestroyZetaSpace()
    {
        Vector3 raycastOrigin = transform.position;
        Vector3 raycastDirection = transform.forward;

        // Dichiarazione del raycast
        RaycastHit hit;

        // Esegui il raycast
        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, 20))
        {
            // Verifica se l'oggetto colpito ha un certo tag o componente, ad esempio
            if (hit.collider.gameObject)
            {
                Debug.Log("Collision");
                Destroy(hit.collider.gameObject);
            }
            else
            {
                // Esegui altre azioni se necessario
            }
        }
        else
        {
            // Se il raycast non colpisce nulla, esegui altre azioni se necessario
        }


    }

    protected override void OnUse()
    {

        // Set the PhotoOutputParent to active and position it according to the object camera
        PhotoOutputParent.SetActive(true);
        PhotoOutputParent.transform.position = CameraObjects.transform.position;
        PhotoOutputParent.transform.rotation = CameraObjects.transform.rotation;

    }
}


