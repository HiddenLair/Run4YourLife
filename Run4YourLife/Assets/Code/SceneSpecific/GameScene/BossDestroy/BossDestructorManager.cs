using Run4YourLife.Utils;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class BossDestructorManager : SingletonMonoBehaviour<BossDestructorManager> {

        private List<BossDestructedInstance> m_staticElementsList = new List<BossDestructedInstance>();

        private BossDestructedInstance[] m_staticElements;

        private int m_bossPositionIndex;

        public void Add(BossDestructedInstance bossDestructedInstance)
        {
            m_staticElementsList.Add(bossDestructedInstance);
        }

        private void Start()
        {
            StartCoroutine(YieldHelper.SkipFrame(BuildStaticElements));
        }

        private void BuildStaticElements()
        {
            m_staticElementsList.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
            m_staticElements = m_staticElementsList.ToArray();
        }

        private void Update()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            if(boss != null)
            {
                DestroyAndRegenerateStaticElements(boss);
            }
        }

        private void DestroyAndRegenerateStaticElements(GameObject boss)
        {
            float xBossPosition = boss.transform.position.x;
            int desiredIndex = EvaluateIndexForBossPosition(xBossPosition);
            Debug.Log(desiredIndex);
            if (desiredIndex < m_bossPositionIndex)
            {
                ResetElementsFromTo(desiredIndex, m_bossPositionIndex);
            }
            else if (desiredIndex > m_bossPositionIndex)
            {
                DestroyElementsFromTo(m_bossPositionIndex, desiredIndex);
            }
            m_bossPositionIndex = desiredIndex;
        }

        private int EvaluateIndexForBossPosition(float xBossPosition)
        {
            //TODO: Optimize me

            /*if(xBossPosition < m_staticElements[0].DestroyPosition)
            {
                return 0;
            }

            for (int i = 0; i < m_staticElements.Length; ++i)
            {
                if (xBossPosition < m_staticElements[i].DestroyPosition)
                {
                    return i-1;
                }
            }

            return m_staticElements.Length - 1;*/

            if(xBossPosition > m_staticElements[m_bossPositionIndex].DestroyPosition)
            {
                while(m_bossPositionIndex < m_staticElements.Length && xBossPosition >= m_staticElements[m_bossPositionIndex].DestroyPosition)
                {
                    m_bossPositionIndex++;
                }
            } else 
            {
                while(m_bossPositionIndex > 0 && xBossPosition < m_staticElements[m_bossPositionIndex-1].DestroyPosition)
                {
                    m_bossPositionIndex--;
                }
            }

            return m_bossPositionIndex;
        }

        private void DestroyElementsFromTo(int fromIndex, float toIndex)
        {
            for (int i = fromIndex; i <= toIndex; i++)
            {
                m_staticElements[i].OnBossDestroy();
            }
        }

        private void ResetElementsFromTo(int fromIndex, int toIndex)
        {
            for (int i = fromIndex; i <= toIndex; i++)
            {
                m_staticElements[i].OnRegenerate();
            }
        }
    }
}