using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Player;

namespace Run4YourLife.SceneManagement
{
    public class LoadByEndRock : MonoBehaviour
    {

        [SerializeField]
        private SceneLoadRequest loadRequest;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == Tags.Runner)
            {
                if (other.GetComponent<RunnerCharacterController>().IsDashing())
                {
                    loadRequest.Execute();
                    Destroy(gameObject);
                }
            }
        }
    }
}
