using UnityEngine;

public class CameraAttributesDefinition : MonoBehaviour
{
    public float desiredFloorHeight;
    public Vector3 desiredPositionOffset;

    // Visible zone APPROXIMATION (no perspective, ...)

    private const float w = 1920.0f;
    private const float h = 1080.0f;
    private const float s = 100.0f;
    private const float fH = 10.0f;

    void OnDrawGizmosSelected()
    {
        Vector3 size = new Vector3(w, h, 1.0f) / s;
        size *= desiredFloorHeight / fH;

        Vector3 position = transform.position + 0.5f * size;
        position += desiredPositionOffset;

        Gizmos.DrawWireCube(position, size);
    }
}