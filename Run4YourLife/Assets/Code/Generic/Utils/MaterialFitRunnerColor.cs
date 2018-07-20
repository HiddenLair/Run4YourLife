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
                case CharacterType.ACorn:
                    color = Color.blue;
                    break;
                case CharacterType.Skull:
                    color = Color.magenta;
                    break;
                case CharacterType.Snake:
                    color = Color.yellow;
                    break;
                case CharacterType.Plain:
                    color = Color.red;
                    break;
                case CharacterType.NoColor:
                    color = Color.gray;
                    break;
            }

            material.color = color;
        }
    }
}