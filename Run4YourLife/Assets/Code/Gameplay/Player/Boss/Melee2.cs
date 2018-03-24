using UnityEngine;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class Melee2 : Melee
    {
        #region Editor variables

        [SerializeField]
        private float instanceSpeed;

        [SerializeField]
        private Transform indicatorPos;

        #endregion

        private AudioSource audioSource;

        protected override void GetComponents()
        {
            base.GetComponents();

            audioSource = GetComponent<AudioSource>();
        }

        protected override void OnSuccess()
        {
            audioSource.PlayOneShot(sfx);

            Vector3 trapPos = indicatorPos.position;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(trapPos);

            if(screenPos.x <= 0.5f * Camera.main.pixelWidth)
            {
                trapPos.x = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, Camera.main.transform.position.z - trapPos.z)).x;
                Instantiate(trapPos, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                trapPos.x = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z - trapPos.z)).x;
                Instantiate(trapPos, Quaternion.Euler(0, 180, 0));
            }
        }

        void Instantiate(Vector3 position, Quaternion rotation)
        {
            GameObject meleeInst = Instantiate(instance, position, rotation);
            meleeInst.GetComponent<Rigidbody>().velocity = meleeInst.transform.right * instanceSpeed * Time.deltaTime;
        }
    }
}