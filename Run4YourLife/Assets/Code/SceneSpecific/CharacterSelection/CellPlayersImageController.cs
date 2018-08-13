using UnityEngine;
using UnityEngine.UI;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class CellPlayersImageController : MonoBehaviour
    {
        [SerializeField]
        private Image[] images;

        [SerializeField]
        private bool scaleOnShow = false;

        public void Show(uint index)
        {
            images[index].enabled = true;

            if(scaleOnShow)
            {
                Scale(index);
            }
        }

        public void Hide(uint index)
        {
            images[index].enabled = false;
        }

        public void Scale(uint index)
        {
            images[index].GetComponent<ScaleOnTick>().Tick();
        }
    }
}