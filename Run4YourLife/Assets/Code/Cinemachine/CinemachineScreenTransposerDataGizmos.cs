using UnityEngine;

namespace Run4YourLife.Cinemachine
{
    public class CinemachineScreenTransposerDataGizmos
    {
        private static readonly float aspectRatio = 16.0f / 9.0f;

        public static void DrawGizmos(CinemachineScreenTransposerData data, Vector3 position)
        {
            Vector3 toRight = new Vector3(data.m_verticalHeight * aspectRatio, 0,0);

            Vector3 bottomLeft = position + data.m_offsetFromTarget;
            Vector3 topLeft = bottomLeft + new Vector3(0,data.m_verticalHeight,0);
            Vector3 bottomRight = bottomLeft + toRight;
            Vector3 topRight = topLeft + toRight;

            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomRight, topRight);
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.DrawLine(topLeft, bottomLeft);
        }
    }
}