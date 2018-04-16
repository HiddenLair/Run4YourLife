using UnityEngine;

using Run4YourLife.SceneManagement;

namespace Run4YourLife.WinMenu
{
    public abstract class WinMenuManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameLoadRequest;

        [SerializeField]
        private SceneLoadRequest characterSelectionLoadRequest;

        [SerializeField]
        private SceneLoadRequest mainMenuLoadRequest;

        public void OnPlayAgainPressed()
        {
            foreach(SceneLoadRequest request in gameLoadRequest.GetComponents<SceneLoadRequest>())
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