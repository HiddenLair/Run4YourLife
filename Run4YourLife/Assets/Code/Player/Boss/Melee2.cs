using UnityEngine;

using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;

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
        private Animator m_animator;
        private Camera m_mainCamera;

        protected override void Awake()
        {
            base.Awake();
            m_animator = GetComponent<Animator>();
            m_mainCamera = CameraManager.Instance.MainCamera;
            crossHairControl = GetComponent<CrossHairControl>();
            Debug.Assert(m_mainCamera != null);
        }

        protected override void OnSuccess()
        {
            //Only shoot when the boss has both hands. No double shoot is allowed.
            if (handR.activeInHierarchy && handL.activeInHierarchy)
            {
                AudioManager.Instance.PlaySFX(m_meleeClip);

                trapPos = crossHairControl.Position;
                Vector3 screenPos = m_mainCamera.WorldToScreenPoint(trapPos);

                if (screenPos.x <= 0.5f * m_mainCamera.pixelWidth)
                {
                    m_animator.SetTrigger("MeleR");
                    StartCoroutine(AnimationCallback.OnStateAtNormalizedTime(m_animator, "MeleRight", timeToMeleFromAnim, () => ArmInstantiate()));
                    trapPos.x = m_mainCamera.ScreenToWorldPoint(new Vector3(m_mainCamera.pixelWidth, 0, m_mainCamera.transform.position.z - trapPos.z)).x;
                    rotation = Quaternion.Euler(0, 0, 0);
                    right = true;
                }
                else
                {
                    m_animator.SetTrigger("MeleL");
                    StartCoroutine(AnimationCallback.OnStateAtNormalizedTime(m_animator, "MeleLeft", timeToMeleFromAnim, () => ArmInstantiate()));
                    trapPos.x = m_mainCamera.ScreenToWorldPoint(new Vector3(0, 0, m_mainCamera.transform.position.z - trapPos.z)).x;
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
                meleeInst.GetComponent<RegenerateHand>().SetHandToRecover(handR);
                handR.SetActive(false);
            }
            else
            {
                meleeInst.GetComponent<RegenerateHand>().SetHandToRecover(handL);
                handL.SetActive(false);
            }
        }
    }
}