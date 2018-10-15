﻿using UnityEngine;

using Run4YourLife.SceneManagement;

namespace Run4YourLife.SceneSpecific.WinMenu
{
    public abstract class WinMenuManager : MonoBehaviour
    {
        [SerializeField]
        protected AudioClip m_characterSound;

        [SerializeField]
        protected AudioClip m_sceneMusic;

        [SerializeField]
        private GameObject gameLoadRequest;

        [SerializeField]
        private SceneTransitionRequest characterSelectionLoadRequest;

        [SerializeField]
        private SceneTransitionRequest mainMenuLoadRequest;

        public void OnPlayAgainPressed()
        {
            foreach(SceneTransitionRequest request in gameLoadRequest.GetComponents<SceneTransitionRequest>())
            {
                request.Execute();
            }
        }

        public void OnGoCharacterSelectionPressed()
        {
            characterSelectionLoadRequest.Execute();
        }

        public void OnGoMainPressed()
        {
            mainMenuLoadRequest.Execute();
        }
    }
}