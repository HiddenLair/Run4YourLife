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

    void Start()
    {
        ComputeTextureTiling();
        Destroy(this);
    }

    private Vector2 GetMeshSize()
    {
        Vector2 size = Vector2.zero;
        Vector3 meshSize = GetComponent<MeshFilter>().sharedMesh.bounds.size;

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
        GetComponent<Renderer>().material.mainTextureScale = meshSize / step;
    }
}