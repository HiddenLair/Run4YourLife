using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(MeshFilter))]
public class AutoTilingAdjust : MonoBehaviour
{
    private enum CoordinateSystem
    {
        XY, XZ, YZ
    }

    [SerializeField]
    private float step = 8.0f;

    [SerializeField]
    private CoordinateSystem coordinateSystem = CoordinateSystem.XZ;

    private new Renderer renderer;
    private MeshFilter meshFilter;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        meshFilter = GetComponent<MeshFilter>();
    }

    void Start()
    {
        ComputeTextureTiling();
    }

    void OnValidate() // Inspector
    {
        // Test the proper step
        // Visualize the changes
        // This can be removed on release

        Awake();
        Start();
    }

    private Vector2 GetMeshSize()
    {
        Vector2 size = Vector2.zero;
        Vector3 meshSize = meshFilter.sharedMesh.bounds.size;

        switch(coordinateSystem)
        {
            case CoordinateSystem.XY:
                size.x = meshSize.x * transform.localScale.x;
                size.y = meshSize.y * transform.localScale.y;
                break;
            case CoordinateSystem.XZ:
                size.x = meshSize.x * transform.localScale.x;
                size.y = meshSize.z * transform.localScale.z;
                break;
            case CoordinateSystem.YZ:
                size.x = meshSize.y * transform.localScale.y;
                size.y = meshSize.z * transform.localScale.z;
                break;
        }

        return size;
    }

    private void ComputeTextureTiling()
    {
        Vector2 meshSize = GetMeshSize();
        renderer.sharedMaterial.mainTextureScale = meshSize / step;
    }
}