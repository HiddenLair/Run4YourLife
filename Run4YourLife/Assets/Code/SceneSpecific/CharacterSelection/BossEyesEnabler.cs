using UnityEngine;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class BossEyesEnabler : MonoBehaviour
    {
        [SerializeField]
        private GameObject left;

        [SerializeField]
        private GameObject right;

        public void Enable(bool value)
        {
            left.SetActive(value);
            right.SetActive(value);
        }
    }
}