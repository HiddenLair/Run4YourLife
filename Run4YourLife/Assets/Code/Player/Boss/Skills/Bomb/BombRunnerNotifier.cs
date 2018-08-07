using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.Player.Boss.Skills.Bomb
{
    public class BombRunnerNotifier : MonoBehaviour
    {
        [SerializeField]
        private BombController m_bombController;

        private void OnValidate()
        {
            Debug.Assert(m_bombController != null, gameObject);
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag(Tags.Runner))
            {
                m_bombController.Explode();
            }
        }
    }
}
