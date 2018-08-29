using UnityEngine;
using UnityEngine.UI;

namespace Run4YourLife.UI
{
    public class SwapImages : MonoBehaviour
    {
        [SerializeField]
        private Sprite spriteA;

        [SerializeField]
        private Sprite spriteB;

        [SerializeField]
        private Image[] images;

        private bool swapped = false;

        public void Swap()
        {
            foreach(Image image in images)
            {
                image.sprite = image.sprite == spriteA ? spriteB : spriteA;
            }

            swapped = !swapped;
        }

        public void ResetInitial()
        {
            foreach(Image image in images)
            {
                image.sprite = spriteA;
            }

            swapped = false;
        }

        public bool Swapped()
        {
            return swapped;
        }
    }
}