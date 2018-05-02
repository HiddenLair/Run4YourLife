using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class Melee2 : Melee
    {
        #region Editor variables

        [SerializeField]
        private GameObject instance;

        [SerializeField]
        private float instanceSpeed;

        [SerializeField]
        private Transform crossHairPos;

        [SerializeField]
        private GameObject handL;

        [SerializeField]
        private GameObject handR;

        #endregion

        private float timeToMeleFromAnim = 0.75f;

        private AudioSource audioSource;
        private Animator anim;

        Vector3 trapPos;
        Quaternion rotation;
        bool right = false;

        protected override void GetComponents()
        {
            base.GetComponents();

            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        protected override void OnSuccess()
        {
            audioSource.PlayOneShot(sfx);

            trapPos = crossHairPos.position;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(trapPos);

            if(screenPos.x <= 0.5f * Camera.main.pixelWidth)
            {
                anim.SetTrigger("MeleR");
                AnimationPlayOnTimeManager.Instance.PlayOnAnimation(anim,"MeleRight",timeToMeleFromAnim,()=>ArmInstantiate());
                trapPos.x = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Camera.main.transform.position.z - trapPos.z)).x;
                rotation = Quaternion.Euler(0, 0, 0);
                right = true;
            }
            else
            {
                anim.SetTrigger("MeleL");
                AnimationPlayOnTimeManager.Instance.PlayOnAnimation(anim, "MeleLeft", timeToMeleFromAnim, () => ArmInstantiate());
                trapPos.x = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z - trapPos.z)).x;
                rotation = Quaternion.Euler(0, 180, 0);
                right = false;
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