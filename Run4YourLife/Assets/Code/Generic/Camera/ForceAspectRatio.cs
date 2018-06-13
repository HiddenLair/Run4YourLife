using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ForceAspectRatio : MonoBehaviour
{
    private const float TARGET_ASPECT_RATIO = 16.0f / 9.0f;

    private Camera m_camera;
    private float previousWindowAspectRatio;

    void Awake()
    {
        m_camera = GetComponent<Camera>();
        previousWindowAspectRatio = -1.0f;
    }

    void Update()
    {
        float currentWindowAspectRatio = (float)Screen.width / (float)Screen.height;

        if(currentWindowAspectRatio != previousWindowAspectRatio)
        {
            Rect rect = m_camera.rect;
            float scaleHeight = currentWindowAspectRatio / TARGET_ASPECT_RATIO;

            if(scaleHeight < 1.0f)
            {
                rect.width = 1.0f; rect.height = scaleHeight;
                rect.x = 0.0f; rect.y = (1.0f - scaleHeight) / 2.0f;
            }
            else
            {
                float scalewidth = 1.0f / scaleHeight;

                rect.width = scalewidth; rect.height = 1.0f;
                rect.x = (1.0f - scalewidth) / 2.0f; rect.y = 0.0f;
            }

            m_camera.rect = rect;
            previousWindowAspectRatio = currentWindowAspectRatio;
        }
    }
}