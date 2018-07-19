using UnityEngine;
using UnityEngine.UI;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class New_CellPlayersImageController : MonoBehaviour
    {
        [SerializeField]
        private Image[] images;

        public void Show(uint index)
        {
            images[index].gameObject.SetActive(true);
        }

        public void Hide(uint index)
        {
            images[index].gameObject.SetActive(false);
        }

        public void HideAll()
        {
            foreach(Image image in images)
            {
                image.gameObject.SetActive(false);
            }
        }
    }
}