using Run4YourLife.Utils;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class BossDestructorManager : SingletonMonoBehaviour<BossDestructorManager> {

        private List<BossDestructedInstance> m_staticElementsList = new List<BossDestructedInstance>();

        private BossDestructedInstance[] m_staticElements;

        private List<BossDestructedInstance> m_dynamicElements = new List<BossDestructedInstance>();

        private int m_bossPositionIndex;

        /// <summary>
        /// Adds a static element. Must be called the first frame the game starts.
        /// Unexpected behaviour otherwise
        /// </summary>
        public void AddStatic(BossDestructedInstance bossDestructedInstance)
        {
            m_staticElementsList.Add(bossDestructedInstance);
        }

        public void AddDynamic(BossDestructedInstance bossDestructedInstance)
        {
            m_dynamicElements.Add(bossDestructedInstance);
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
                DestroyAndRegenerateDynamicElements(boss);
                DestroyAndRegenerateStaticElements(boss);
            }
        }

        private void DestroyAndRegenerateDynamicElements(GameObject boss)
        {
            float xBossPosition = boss.transform.position.x;
            foreach(BossDestructedInstance bossDestructedInstance in m_dynamicElements)
            {
                if(xBossPosition < bossDestructedInstance.DestroyPosition)
                {
                    if(bossDestructedInstance.IsDestructed)
                    {
                        bossDestructedInstance.OnRegenerate();
                    }
                }
                else
                {
                    if(!bossDestructedInstance.IsDestructed)
                    {
                        bossDestructedInstance.OnBossDestroy();
                    }
                }
            }
        }

        private void DestroyAndRegenerateStaticElements(GameObject boss)
        {
            float xBossPosition = boss.transform.position.x;
            int lastSmaller = LastElementSmallerThanPosition(xBossPosition);

            if (lastSmaller > m_bossPositionIndex)
            {
                DestroyElementsFromTo(m_bossPositionIndex + 1, lastSmaller);
            }
            else if (lastSmaller < m_bossPositionIndex)
            {
                ResetElementsFromTo(lastSmaller + 1, m_bossPositionIndex);
            }
            m_bossPositionIndex = lastSmaller;
        }

        private int LastElementSmallerThanPosition(float position)
        {
            int firstBigger = 0;
            while(firstBigger < m_staticElements.Length && m_staticElements[firstBigger].DestroyPosition < position)
            {
                firstBigger++;
            }
            return firstBigger - 1;
        }

        private void DestroyElementsFromTo(int fromIndex, float toIndex)
        {
            for (int i = fromIndex; i <= toIndex; i++)
            {
                if(i >= 0)
                {
                    m_staticElements[i].OnBossDestroy();
                }
            }
        }

        private void ResetElementsFromTo(int fromIndex, int toIndex)
        {
            for (int i = fromIndex; i <= toIndex; i++)
            {
                if(i >= 0)
                {
                    m_staticElements[i].OnRegenerate();
                }
            }
        }
    }
}