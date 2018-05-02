using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Interactables
{
    public class MoveRunnersWithYou : MonoBehaviour
    {

        private Vector3 m_previousPosition;

        List<Transform> players = new List<Transform>();

        void FixedUpdate()
        {
            Vector3 delta = transform.position - m_previousPosition;

            foreach (Transform transform in players)
            {
                transform.Translate(delta, Space.World);
            }

            m_previousPosition = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                players.Add(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                players.Remove(other.transform);
            }
        }
    }
}