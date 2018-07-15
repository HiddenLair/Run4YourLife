using UnityEngine;

using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;
using Run4YourLife.CameraUtils;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CrossHairControl))]
    public class Melee2 : Melee
    {
        #region Editor variables

        [SerializeField]
        private GameObject instance;

        [SerializeField]
        private float instanceSpeed;

        [SerializeField]
        private GameObject handL;

        [SerializeField]
        private GameObject handR;

        #endregion

        private float timeToMeleFromAnim = 0.75f;

        private Vector3 trapPos;
        private Quaternion rotation;
        private bool right = false;

        private CrossHairControl crossHairControl;
        private Camera m_mainCamera;

        protected override void Awake()
        {
            base.Awake();
            m_mainCamera = CameraManager.Instance.MainCamera;
            crossHairControl = GetComponent<CrossHairControl>();
            Debug.Assert(m_mainCamera != null);
        }

        protected override void ExecuteMelee()
        {
            //Only shoot when the boss has both hands. No double shoot is allowed.
            if (handR.activeInHierarchy && handL.activeInHierarchy)
            {
                AudioManager.Instance.PlaySFX(m_meleeClip);

                trapPos = crossHairControl.Position;
                Vector3 screenPos = m_mainCamera.WorldToViewportPoint(trapPos);

                if (screenPos.x <= 0.5f)
                {
                    m_animator.SetTrigger("MeleR");
                    StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(m_animator, "MeleRight", timeToMeleFromAnim, () => ArmInstantiate()));
                    trapPos.x = CameraConverter.ViewportToGamePlaneWorldPosition(m_mainCamera, new Vector2(1,0)).x;
                    rotation = Quaternion.Euler(0, 0, 0);
                    right = true;
                }
                else
                {
                    m_animator.SetTrigger("MeleL");
                    StartCoroutine(AnimationCallbacks.OnStateAtNormalizedTime(m_animator, "MeleLeft", timeToMeleFromAnim, () => ArmInstantiate()));
                    trapPos.x = CameraConverter.ViewportToGamePlaneWorldPosition(m_mainCamera, new Vector2(0,0)).x;
                    rotation = Quaternion.Euler(0, 180, 0);
                    right = false;
                }
            }
        }

        void ArmInstantiate()
        {
            GameObject meleeInst = Instantiate(instance, trapPos, rotation);
            meleeInst.GetComponent<Rigidbody>().velocity = meleeInst.transform.right * instanceSpeed * Time.deltaTime;
            if (right)
            {
                //meleeInst.GetComponent<RegenerateHand>().SetHandToRecover(handR);
                handR.SetActive(false);
            }
            else
            {
                //meleeInst.GetComponent<RegenerateHand>().SetHandToRecover(handL);
                handL.SetActive(false);
            }
        }
    }
}