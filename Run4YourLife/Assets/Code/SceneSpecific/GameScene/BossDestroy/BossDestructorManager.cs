using Run4YourLife.Utils;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class BossDestructorManager : SingletonMonoBehaviour<BossDestructorManager> {

        private List<BossDestructedInstance> m_staticElementsList = new List<BossDestructedInstance>();

        private BossDestructedInstance[] m_staticElements;

        private List<BossDestructedInstance> m_destroyedDynamicElements = new List<BossDestructedInstance>();

        private List<BossDestructedInstance> m_activeDynamicElements = new List<BossDestructedInstance>();

        ///<summay>
        /// Used just to hold references and not create garbage each frame
        ///</summary>
        private List<BossDestructedInstance> m_temporalActiveDynamicElements = new List<BossDestructedInstance>();

        private int m_bossPositionIndex;

        /// <summary>
        /// Adds a static element. Must be called the first frame the game starts.
        /// Unexpected behaviour otherwise
        /// </summary>
        public void AddStatic(BossDestructedInstance bossDestructedInstance)
        {
            Debug.Assert(m_staticElements == null); // we can only add elements the first frame otherwise we would have to rebuild the array
            m_staticElementsList.Add(bossDestructedInstance);
        }

        public void AddDynamic(BossDestructedInstance bossDestructedInstance)
        {
            if(GameplayPlayerManager.Instance.Boss == null)
            {
                m_activeDynamicElements.Add(bossDestructedInstance);
            }
            else if(bossDestructedInstance.DestroyPosition <= GameplayPlayerManager.Instance.Boss.transform.position.x)
            {
                m_destroyedDynamicElements.Add(bossDestructedInstance);
                bossDestructedInstance.OnBossDestroy();
            }
            else
            {
                m_activeDynamicElements.Add(bossDestructedInstance);
            }
        }

        public void RemoveDynamic(BossDestructedInstance bossDestructedInstance)
        {
            if(!m_destroyedDynamicElements.Remove(bossDestructedInstance))
            {
                Debug.Assert(m_activeDynamicElements.Remove(bossDestructedInstance));
            }
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
                float xBossPosition = boss.transform.position.x;
                DestroyAndRegenerateDynamicElements(xBossPosition);
                DestroyAndRegenerateStaticElements(xBossPosition);
            }
        }

        private void DestroyAndRegenerateDynamicElements(float xBossPosition)
        {
            foreach(BossDestructedInstance bossDestructedInstance in m_destroyedDynamicElements)
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

            // Reverse loops are more efficient when we need to traverse and delete
            if(m_destroyedDynamicElements.Count > 0)
            {
                for (int i = m_destroyedDynamicElements.Count - 1; i >= 0 ; i--) 
                {
                    BossDestructedInstance bossDestructedInstance = m_destroyedDynamicElements[i];
                    if(xBossPosition > bossDestructedInstance.DestroyPosition)
                    {
                        // we do not add them directly to avoid unnecesary removing cost for the next operation
                        m_temporalActiveDynamicElements.Add(bossDestructedInstance);
                        m_destroyedDynamicElements.RemoveAt(i);
                        bossDestructedInstance.OnRegenerate();
                    }
                }
            }

            if(m_activeDynamicElements.Count > 0)
            {
                for (int i = m_activeDynamicElements.Count - 1; i >= 0 ; i--) 
                {
                    BossDestructedInstance bossDestructedInstance = m_activeDynamicElements[i];
                    if(xBossPosition <= bossDestructedInstance.DestroyPosition)
                    {
                        m_activeDynamicElements.RemoveAt(i);
                        bossDestructedInstance.OnBossDestroy();
                        m_destroyedDynamicElements.Add(bossDestructedInstance);
                    }
                }
            }

            m_activeDynamicElements.AddRange(m_temporalActiveDynamicElements);
            m_temporalActiveDynamicElements.Clear();
        }

        private void DestroyAndRegenerateStaticElements(float xBossPosition)
        {
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