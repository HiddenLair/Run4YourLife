using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Interactables
{
    public class MovingPlatform : MonoBehaviour
    {
        public Transform[] m_wayPoints;
        public float speed;

        private int m_direction = 1;

        private int m_currentDestinyIndex = 0;

        private Rigidbody m_rigidbody;

        private Vector3 m_previousPosition;

        List<Transform> players = new List<Transform>();

        private void Start()
        {
            m_previousPosition = transform.position;
            m_rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (m_wayPoints.Length > 0)
            {
                if (transform.position.Equals(m_wayPoints[m_currentDestinyIndex].position))
                {
                    NextIndex();
                }
                m_rigidbody.MovePosition(Vector3.MoveTowards(transform.position, m_wayPoints[m_currentDestinyIndex].position, speed*Time.fixedDeltaTime));

                Vector3 delta = transform.position - m_previousPosition;

                foreach (Transform transform in players)
                {
                    transform.Translate(delta, Space.World);
                }

                m_previousPosition = transform.position;
            }
        }

        private void NextIndex()
        {
            m_currentDestinyIndex += m_direction;

            if (m_currentDestinyIndex == m_wayPoints.Length || m_currentDestinyIndex < 0)
            {
                m_direction *= -1;
                m_currentDestinyIndex += m_direction*2;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            players.Add(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            players.Remove(other.transform);
        }
    }
}