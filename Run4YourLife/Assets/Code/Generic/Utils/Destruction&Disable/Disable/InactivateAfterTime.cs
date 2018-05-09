using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Utils
{
    public class InactivateAfterTime : MonoBehaviour {

        [SerializeField]
        private float m_time;
        
        private float m_disableTime;

        private void OnEnable()
        {
            m_disableTime = Time.time + m_time;
        }

        private void Update()
        {
            if(m_disableTime <= Time.time)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
