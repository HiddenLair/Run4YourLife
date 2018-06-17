using UnityEngine;
using Cinemachine;

namespace Run4YourLife.GameManagement
{
    public class CameraManager : SingletonMonoBehaviour<CameraManager> {

        public ICinemachineCamera ActiveCinemachineCamera { get { return CinemachineBrain.ActiveVirtualCamera; } }

        public Camera MainCamera {
            get {
                if(m_mainCamera == null)
                {
                    m_mainCamera = Camera.main;
                    Debug.Assert(m_mainCamera != null);
                }
                return m_mainCamera;
            }
        }

        public CinemachineBrain CinemachineBrain {
            get {
                if (m_cinemachineBrain == null)
                {
                    m_cinemachineBrain = MainCamera.GetComponent<CinemachineBrain>();
                    Debug.Assert(m_cinemachineBrain != null);
                }
                return m_cinemachineBrain;
            }
        }

        private Camera m_mainCamera;
        private CinemachineBrain m_cinemachineBrain;

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

