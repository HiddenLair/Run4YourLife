using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameInput;

namespace Run4YourLife.CharacterSelection
{
    public class PlayerDetector : MonoBehaviour {

        private PlayerInput playerInput;

        void Awake()
        {
            playerInput = Component.FindObjectOfType<PlayerInput>();
        }

        void Update()
        {

        }
    }
}