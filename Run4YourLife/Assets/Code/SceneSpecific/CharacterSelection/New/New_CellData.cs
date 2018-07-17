using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.SceneSpecific.CharacterSelection
{
    public class New_CellData : MonoBehaviour
    {
        public bool isBoss;
        public CharacterType characterType;

        public New_CellData navigationUp;
        public New_CellData navigationDown;
        public New_CellData navigationLeft;
        public New_CellData navigationRight;

        public string animationNameOnSelected;
        public string animationNameOnNotSelected;
    }
}