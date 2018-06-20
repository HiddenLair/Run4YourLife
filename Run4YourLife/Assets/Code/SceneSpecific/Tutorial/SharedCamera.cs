using UnityEngine;

public class SharedCamera : MonoBehaviour {

    [SerializeField]
    private float yPosition;

    [SerializeField]
    private float boundingBoxPadding = 1.0f;

    [SerializeField]
    private float minimumZDistance;

    [SerializeField]
    private float maximumZDistance;

    [SerializeField]
    private float zoomSpeed = 20f;

    private Camera cam;
    private Transform[] targets;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void SetTargets(Transform[] newTargets)
    {
        targets = newTargets;
    }

    void LateUpdate()
    {
        Rect boundingBox = CalculateTargetsBoundingBox();
        Vector3 pos;
        pos.x = boundingBox.center.x;
        pos.y = yPosition;
        pos.z = Mathf.Clamp((minimumZDistance - CalculateOrthographicSize(boundingBox)), maximumZDistance, minimumZDistance);
        transform.position = pos;
    }

    /// <summary>
    /// Calculates a bounding box that contains all the targets.
    /// </summary>
    /// <returns>A Rect containing all the targets.</returns>
    Rect CalculateTargetsBoundingBox()
    {
        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;
        float minY = Mathf.Infinity;
        float maxY = Mathf.NegativeInfinity;

        foreach (Transform target in targets)
        {
            Vector3 position = target.position;

            minX = Mathf.Min(minX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);
        }

        return Rect.MinMaxRect(minX - boundingBoxPadding, maxY + boundingBoxPadding, maxX + boundingBoxPadding, minY - boundingBoxPadding);
    }

    /// <summary>
    /// Calculates a new orthographic size for the camera based on the target bounding box.
    /// </summary>
    /// <param name="boundingBox">A Rect bounding box containg all targets.</param>
    /// <returns>A float for the orthographic size.</returns>
    float CalculateOrthographicSize(Rect boundingBox)
    {
        float orthographicSize = cam.orthographicSize;
        Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
        Vector3 topRightAsViewport = cam.WorldToViewportPoint(topRight);

        if (topRightAsViewport.x >= topRightAsViewport.y)
            orthographicSize = Mathf.Abs(boundingBox.width) / cam.aspect / 2f;
        else
            orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

        return (orthographicSize / Mathf.Tan((Camera.main.fieldOfView * Mathf.Deg2Rad) / 2f));
    }
}
