using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.SceneManagement
{
    public class LoadSceneOnDestroy : MonoBehaviour
    {

        [SerializeField]
        private SceneLoadRequest loadRequest;

        private void OnDestroy()
        {
            loadRequest.Execute();
        }
    }
}
