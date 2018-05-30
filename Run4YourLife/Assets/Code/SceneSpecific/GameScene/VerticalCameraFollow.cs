using UnityEngine;

using Run4YourLife.GameManagement;

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
            float bottomYCameraPosition = m_mainCamera.ScreenToWorldPoint(new Vector3(0, 0, Mathf.Abs(m_mainCamera.transform.position.z - transform.position.z))).y;
            
            Vector3 newPos = transform.position;
            newPos.y  = bottomYCameraPosition + m_offset;
            
            transform.position = newPos;
        }
    }
 
}
