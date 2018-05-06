using Run4YourLife.Utils;
using System.Collections.Generic;

using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class BossDestructorManager : SingletonMonoBehaviour<BossDestructorManager> {

        private List<BossDestructedInstance> m_staticElementsList = new List<BossDestructedInstance>();

        private BossDestructedInstance[] m_staticElements;
        private int bossPositionIndex;

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
            if(GameplayPlayerManager.Instance.Boss)
            {
                float xBossPosition = GameplayPlayerManager.Instance.Boss.transform.position.x;
                int desiredIndex = EvaluateIndexForBossPosition(xBossPosition);

                if (desiredIndex < bossPositionIndex)
                {
                    ResetElementsFromTo(desiredIndex+1, bossPositionIndex);
                }
                else if (desiredIndex != bossPositionIndex)
                {
                    DestroyElementsFromTo(bossPositionIndex, desiredIndex);
                }
                bossPositionIndex = desiredIndex;
            }
        }

        private int EvaluateIndexForBossPosition(float xBossPosition)
        {
            //TODO: Optimize me

            if(xBossPosition < m_staticElements[0].DestroyPosition)
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

            return m_staticElements.Length - 1;
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