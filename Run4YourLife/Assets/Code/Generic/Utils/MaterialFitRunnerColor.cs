using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.Utils
{
    [RequireComponent(typeof(MeshRenderer))]
    public class MaterialFitRunnerColor : MonoBehaviour
    {
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
                case CharacterType.Red:
                    color = Color.red;
                    break;
                case CharacterType.Green:
                    color = Color.green;
                    break;
                case CharacterType.White:
                    color = Color.white;
                    break;
                case CharacterType.Purple:
                    color = Color.magenta;
                    break;
            }

            material.color = color;
        }
    }
}