using UnityEngine;
using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class CameraManager : SingletonMonoBehaviour<CameraManager> {

        public Camera MainCamera { get { return m_camera; } }
        public CinemachineBrain CinemachineBrain { get { return m_cinemachineBrain; } }
        public ICinemachineCamera ActiveCinemachineCamera { get { return m_cinemachineBrain.ActiveVirtualCamera; } }

        private Camera m_camera;
        private CinemachineBrain m_cinemachineBrain;

        private void Awake()
        {
            m_camera = Camera.main;
            Debug.Assert(m_camera != null);

            m_cinemachineBrain = m_camera.GetComponent<CinemachineBrain>();
            Debug.Assert(m_cinemachineBrain != null);
        }

        public void TransitionToCamera(ICinemachineCamera cinemachineCamera)
        {
            if(cinemachineCamera != ActiveCinemachineCamera)
            {
                if (ActiveCinemachineCamera != null)
                {
                    ActiveCinemachineCamera.LookAt = null;
                    ActiveCinemachineCamera.Follow = null;
                    ActiveCinemachineCamera.VirtualCameraGameObject.SetActive(false);
                }

                cinemachineCamera.VirtualCameraGameObject.SetActive(true);
            }
        }
    }
}

