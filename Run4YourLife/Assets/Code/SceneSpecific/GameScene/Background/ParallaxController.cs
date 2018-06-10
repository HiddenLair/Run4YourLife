using UnityEngine;

using Run4YourLife.GameManagement;

[RequireComponent(typeof(Renderer))]
public class ParallaxController : MonoBehaviour
{
    /*

    Use of texture offset for h movement
    Use of transform position for v movement

    */

    [SerializeField]
    private float hSpeed;

    [SerializeField]
    private float vSpeed;

    private Material material;

    private float initialX;
    private float initialY;
    private float cameraInitialX;
    private float cameraInitialY;
    private Camera storedMainCamera;

    void Awake()
    {
        material = GetComponent<Renderer>().material;

        initialX = transform.position.x;
        initialY = transform.position.y;
    }

    void Update()
    {
        UpdateStoredCamera();

        UpdateHMovement();
        UpdateVMovement();
    }

    private void UpdateStoredCamera()
    {
        Camera camera = CameraManager.Instance.MainCamera;

        if(storedMainCamera != camera)
        {
            storedMainCamera = camera;

            cameraInitialX = camera.transform.position.x;
            cameraInitialY = camera.transform.position.y;
        }
    }

    private void UpdateHMovement()
    {
        float cameraAmountX = CameraManager.Instance.MainCamera.transform.position.x - cameraInitialX;
        float currentX = hSpeed * cameraAmountX;

        material.mainTextureOffset = Vector2.right * (currentX - initialX);
    }

    private void UpdateVMovement()
    {
        float cameraAmountY = CameraManager.Instance.MainCamera.transform.position.y - cameraInitialY;
        float currentY = vSpeed * cameraAmountY;

        Vector3 newPosition = transform.position;
        newPosition.y = initialY - currentY;
        transform.position = newPosition;
    }
}