using UnityEngine;

namespace Run4YourLife.Utils
{
    public class ChangeRigidBodyOnCollision : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody body;

        public float delayActivation;
        public bool gravity;
        public bool kinematic;

        private bool activatedFlag;

        private void OnCollisionEnter(Collision collision)
        {
            if (!activatedFlag)
            {
                // StartCoroutine(ChangeDelayed());
                StartCoroutine(YieldHelper.WaitForSeconds(Change, delayActivation));
            }
            activatedFlag = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!activatedFlag)
            {
                // StartCoroutine(ChangeDelayed());
                StartCoroutine(YieldHelper.WaitForSeconds(Change, delayActivation));
            }
            activatedFlag = true;
        }

        private void Change()
        {
            body.useGravity = gravity;
            body.isKinematic = kinematic;
        }
    }
}