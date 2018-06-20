using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class BossTutorial : GamePhaseManager
    {
        public override GamePhase GamePhase { get { return GamePhase.BossTutorial; } }

        [SerializeField]
        private Vector3 fixCameraPosition;

        [SerializeField]
        private float timeToChangeCamera;

        private PlayerSpawner m_playerSpawner;

        private void Awake()
        {
            m_playerSpawner = GetComponent<PlayerSpawner>();
            Debug.Assert(m_playerSpawner != null);

        }

        public override void StartPhase()
        {
            Camera cam = CameraManager.Instance.MainCamera;
            SharedCamera script = cam.GetComponent<SharedCamera>();
            script.FixCameraAtPosition(fixCameraPosition,timeToChangeCamera,()=>SpawnBoss());
        }

        private void SpawnBoss()
        {
            m_playerSpawner.ActivateBoss();
        }

        public override void EndPhase()
        {
            
        }

        public override void DebugStartPhase()
        {
            Debug.LogError("This method should never be called");
        }

        public override void DebugEndPhase()
        {
            Debug.LogError("This method should never be called");
        }
    }
}