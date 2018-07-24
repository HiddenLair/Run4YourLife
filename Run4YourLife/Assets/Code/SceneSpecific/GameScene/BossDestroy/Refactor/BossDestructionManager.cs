using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Utils;


namespace Run4YourLife.GameManagement.Refactor
{
    public class BossDestructionManager : SingletonMonoBehaviour<BossDestructionManager>
    {
        //Static Destructibles
        private List<IBossDestructible> m_staticDestructiblesList = new List<IBossDestructible>();
        private IBossDestructible[] m_staticDestructibles;
        private int m_bossPositionIndex;


        //Dynamic Destructibles
        private List<IBossDestructible> m_destroyedDynamicDestructibles = new List<IBossDestructible>();
        private List<IBossDestructible> m_activeDynamicDestructibles = new List<IBossDestructible>();
        ///<summay>
        /// Used just to hold references and not create garbage each frame
        ///</summary>
        private List<IBossDestructible> m_temporalActiveDynamicDestructibles = new List<IBossDestructible>();


        /// <summary>
        /// Adds a static element. Must be called the first frame the game starts.
        /// Unexpected behaviour otherwise
        /// </summary>
        public void AddStatic(IBossDestructible bossDestructible)
        {
            Debug.Assert(m_staticDestructibles == null); // we can only add elements the first frame otherwise we would have to rebuild the array
            m_staticDestructiblesList.Add(bossDestructible);
        }

        public void AddDynamic(IBossDestructible bossDestructible)
        {
            if (GameplayPlayerManager.Instance.Boss == null || bossDestructible.DestroyPosition > GameplayPlayerManager.Instance.Boss.transform.position.x || GameManager.Instance.GamePhase == GamePhase.BossFight)
            {
                m_activeDynamicDestructibles.Add(bossDestructible);
            }
            else
            {
                m_destroyedDynamicDestructibles.Add(bossDestructible);
                bossDestructible.Destroy();
            }
        }

        public void RemoveDynamic(IBossDestructible bossDestructible)
        {
            if (!m_destroyedDynamicDestructibles.Remove(bossDestructible))
            {
                Debug.Assert(m_activeDynamicDestructibles.Remove(bossDestructible));
            }
        }

        private void Start()
        {
            StartCoroutine(YieldHelper.SkipFrame(BuildStaticElements));
        }

        private void BuildStaticElements()
        {
            m_staticDestructiblesList.Sort((a, b) => a.DestroyPosition.CompareTo(b.DestroyPosition));
            m_staticDestructibles = m_staticDestructiblesList.ToArray();
            m_staticDestructiblesList = null;
        }

        private void Update()
        {
            GameObject boss = GameplayPlayerManager.Instance.Boss;
            if (boss != null)
            {
                float xBossPosition = boss.transform.position.x;
                DestroyAndRegenerateDynamicElements(xBossPosition);
                DestroyAndRegenerateStaticElements(xBossPosition);
            }
        }

        private void DestroyAndRegenerateDynamicElements(float xBossPosition)
        {
            if (m_destroyedDynamicDestructibles.Count > 0)
            {
                for (int i = m_destroyedDynamicDestructibles.Count - 1; i >= 0; i--)
                {
                    IBossDestructible bossDestructedInstance = m_destroyedDynamicDestructibles[i];
                    if (xBossPosition < bossDestructedInstance.DestroyPosition)
                    {
                        bossDestructedInstance.Regenerate();
                        m_temporalActiveDynamicDestructibles.Add(bossDestructedInstance);
                        m_destroyedDynamicDestructibles.RemoveAt(i);
                    }
                }
            }


            if (m_activeDynamicDestructibles.Count > 0)
            {
                for (int i = m_activeDynamicDestructibles.Count - 1; i >= 0; i--)
                {
                    IBossDestructible bossDestructedInstance = m_activeDynamicDestructibles[i];
                    if (xBossPosition >= bossDestructedInstance.DestroyPosition)
                    {
                        bossDestructedInstance.Destroy();
                        m_activeDynamicDestructibles.RemoveAt(i);
                        m_destroyedDynamicDestructibles.Add(bossDestructedInstance);
                    }
                }
            }

            m_activeDynamicDestructibles.AddRange(m_temporalActiveDynamicDestructibles);
            m_temporalActiveDynamicDestructibles.Clear();
        }

        private void DestroyAndRegenerateStaticElements(float xBossPosition)
        {
            int lastSmaller = LastElementSmallerThanPosition(xBossPosition);

            if (lastSmaller > m_bossPositionIndex)
            {
                DestroyStaticDestructiblesFromTo(m_bossPositionIndex + 1, lastSmaller);
            }
            else if (lastSmaller < m_bossPositionIndex)
            {
                RegenerateStaticDestructiblesFromTo(lastSmaller + 1, m_bossPositionIndex);
            }
            m_bossPositionIndex = lastSmaller;
        }

        private int LastElementSmallerThanPosition(float position)
        {
            int firstBigger = 0;
            while (firstBigger < m_staticDestructibles.Length && m_staticDestructibles[firstBigger].DestroyPosition < position)
            {
                firstBigger++;
            }
            return firstBigger - 1;
        }

        private void DestroyStaticDestructiblesFromTo(int fromIndex, float toIndex)
        {
            for (int i = Mathf.Max(fromIndex, 0); i <= toIndex; i++)
            {
                m_staticDestructibles[i].Destroy();
            }
        }

        private void RegenerateStaticDestructiblesFromTo(int fromIndex, int toIndex)
        {
            for (int i = Mathf.Max(fromIndex, 0); i <= toIndex; i++)
            {
                m_staticDestructibles[i].Regenerate();
            }
        }
    }
}