using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.Utils
{
    public class TransformLockedInPosition : MonoBehaviour
    {
        #region Inspector
        [SerializeField]
        private Vector3 worldPosition;

        [SerializeField]
        private bool setX;

        [SerializeField]
        private bool setY;

        [SerializeField]
        private bool setZ;
        #endregion

        #region Variables

        private Transform m_transform;

        #endregion

        private void Awake()
        {
            m_transform = GetComponent<Transform>();
            UpdatePosition();
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            Vector3 tempPosition = m_transform.position;
            if (setX)
            {
                tempPosition.x = worldPosition.x;
            }
            if (setY)
            {
                tempPosition.y = worldPosition.y;
            }
            if (setZ)
            {
                tempPosition.z = worldPosition.z;
            }
            m_transform.position = tempPosition;
        }
    }
}
