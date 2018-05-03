using UnityEngine;
using Run4YourLife.Cinemachine;

public class CinemachineScreenTransposerDataHolder : MonoBehaviour
{
    [SerializeField]
    private CinemachineScreenTransposerData m_cinemachineScreenTransposerData;

    public CinemachineScreenTransposerData CinemachineScreenTransposerData { get { return m_cinemachineScreenTransposerData; } }

    // Visible zone APPROXIMATION (no perspective, ...)
    private static readonly float w = 1920.0f;
    private static readonly float h = 1080.0f;
    private static readonly float s = 100.0f;
    private static readonly float fH = 10.0f;

    void OnDrawGizmosSelected()
    {
        Vector3 size = new Vector3(w, h, 1.0f) / s;
        size *= m_cinemachineScreenTransposerData.m_verticalHeight / fH;

        Vector3 position = transform.position + 0.5f * size;
        position += m_cinemachineScreenTransposerData.m_offsetFromTarget;

        Gizmos.DrawWireCube(position, size);
    }
}