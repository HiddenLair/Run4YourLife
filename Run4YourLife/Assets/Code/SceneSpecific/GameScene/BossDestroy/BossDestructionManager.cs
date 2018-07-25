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
        private List<IBossDestructible> m_dynamicDestructibles = new List<IBossDestructible>();


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
            m_dynamicDestructibles.Add(bossDestructible);
        }

        public void RemoveDynamic(IBossDestructible bossDestructible)
        {
            Debug.Assert(m_dynamicDestructibles.Remove(bossDestructible));
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
            foreach (IBossDestructible bossDestructible in m_dynamicDestructibles)
            {
                switch (bossDestructible.BossDestructibleState)
                {
                    case BossDestructibleState.Alive:
                        if (bossDestructible.DestroyPosition <= xBossPosition)
                        {
                            bossDestructible.Destroy();
                        }
                        break;

                    case BossDestructibleState.InDestruction:
                    case BossDestructibleState.Destroyed:
                        if (bossDestructible.DestroyPosition > xBossPosition)
                        {
                            bossDestructible.Regenerate();
                        }
                        break;
                }
            }
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
                IBossDestructible bossDestructible = m_staticDestructibles[i];
                if (bossDestructible.BossDestructibleState == BossDestructibleState.Alive)
                {
                    m_staticDestructibles[i].Destroy();
                }
            }
        }

        private void RegenerateStaticDestructiblesFromTo(int fromIndex, int toIndex)
        {
            for (int i = Mathf.Max(fromIndex, 0); i <= toIndex; i++)
            {
                IBossDestructible bossDestructible = m_staticDestructibles[i];
                if (bossDestructible.BossDestructibleState == BossDestructibleState.Destroyed || bossDestructible.BossDestructibleState == BossDestructibleState.InDestruction)
                {
                    m_staticDestructibles[i].Regenerate();
                }
            }
        }
    }
}