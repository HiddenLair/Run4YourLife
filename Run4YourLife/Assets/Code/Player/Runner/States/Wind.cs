using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class Wind : MonoBehaviour
    {
        private float m_windForce;
        private HashSet<WindSkillControl> m_windSkillControls = new HashSet<WindSkillControl>();

        public float GetWindForce()
        {
            return m_windForce;
        }

        public void Destroy()
        {
            Destroy(this);
        }

        private void Update()
        {
            m_windSkillControls.RemoveWhere(w => !w.gameObject.activeInHierarchy);

            if (m_windSkillControls.Count == 0)
            {
                Destroy(this);
            }
        }

        public void EnterWindArea(WindSkillControl windSkillControl)
        {
            m_windForce += windSkillControl.GetWindForce();
            m_windSkillControls.Add(windSkillControl);
        }

        public void ExitWindArea(WindSkillControl windSkillControl)
        {
            m_windForce -= windSkillControl.GetWindForce();
            m_windSkillControls.Remove(windSkillControl);

            if (m_windSkillControls.Count == 0)
            {
                Destroy(this);
            }
        }
    }
}