using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ForceAspectRatio : MonoBehaviour
{
    private const float TARGET_ASPECT_RATIO = 16.0f / 9.0f;

    private new Camera camera;
    private float previousWindowAspectRatio;

    void Awake()
    {
        camera = GetComponent<Camera>();
        previousWindowAspectRatio = -1.0f;
    }

    void Update()
    {
        float currentWindowAspectRatio = (float)Screen.width / (float)Screen.height;

        if(currentWindowAspectRatio != previousWindowAspectRatio)
        {
            float scaleHeight = currentWindowAspectRatio / TARGET_ASPECT_RATIO;

            if(scaleHeight <= 1.0f)
            {
                Rect rect = camera.rect;

                rect.width = 1.0f; rect.height = scaleHeight;
                rect.x = 0.0f; rect.y = (1.0f - scaleHeight) / 2.0f;

                camera.rect = rect;
            }
            else
            {
                Debug.LogWarning("ForceAspectRatio::Update() => scaleHeight > 1.0f");
            }

            previousWindowAspectRatio = currentWindowAspectRatio;
        }
    }
}