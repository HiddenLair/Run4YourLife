using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Utils
{
    [RequireComponent(typeof(MeshRenderer))]
    public class MaterialFitRunnerColor : MonoBehaviour
    {
        private static Color red = Color.red;
        private static Color green = Color.green;
        private static Color white = Color.white;
        private static Color purple = new Color(1.0f, 0.0f, 1.0f);

        private Material material;

        void Awake()
        {
            material = GetComponent<MeshRenderer>().material;
        }

        public void Activate(CharacterType characterType)
        {
            Color color = Color.black;

            switch(characterType)
            {
                case CharacterType.Green:
                    color = red;
                    break;
                case CharacterType.Blue:
                    color = green;
                    break;
                case CharacterType.Purple:
                    color = white;
                    break;
                case CharacterType.Orange:
                    color = purple;
                    break;
            }

            material.color = color;
        }
    }
}