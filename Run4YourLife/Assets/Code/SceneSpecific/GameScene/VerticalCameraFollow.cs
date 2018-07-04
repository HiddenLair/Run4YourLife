using UnityEngine;

using Run4YourLife.GameManagement;
using Run4YourLife.CameraUtils;

namespace Run4YourLife.SceneSpecific.GameScene
{
    public class VerticalCameraFollow : MonoBehaviour {

        [SerializeField]
        private float m_offset;

        private Camera m_mainCamera;

        void Start () {
            m_mainCamera = CameraManager.Instance.MainCamera;
        }
        
        // Update is called once per frame
        void Update () {
            float bottomYCameraPosition = CameraConverter.ViewportToGamePlaneWorldPosition(m_mainCamera, new Vector2(0.5f, 0)).y;

            Vector3 newPos = transform.position;
            newPos.y  = bottomYCameraPosition + m_offset;
            
            transform.position = newPos;
        }
    }
 
}
