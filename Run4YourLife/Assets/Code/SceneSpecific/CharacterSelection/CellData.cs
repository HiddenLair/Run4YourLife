using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class CellData : MonoBehaviour
    {
        public bool isBoss;
        public CharacterType characterType;

        public CellData navigationUp;
        public CellData navigationDown;
        public CellData navigationLeft;
        public CellData navigationRight;

        public string animationNameOnSelected;
        public string animationNameOnNotSelected;
    }
}