using UnityEngine;
using Run4YourLife.Cinemachine;

public class CinemachineScreenTransposerDataGizmos
{
    // Visible zone APPROXIMATION (no perspective, ...)
    private static readonly float w = 1920.0f;
    private static readonly float h = 1080.0f;
    private static readonly float s = 100.0f;
    private static readonly float fH = 10.0f;

    public static void DrawGizmos(CinemachineScreenTransposerData data, Vector3 position)
    {
        Vector3 size = new Vector3(w, h, 1.0f) / s;
        size *= data.m_verticalHeight / fH;

        Vector3 center = position + 0.5f * size;
        center += data.m_offsetFromTarget;

        Gizmos.DrawWireCube(center, size);
    }
}