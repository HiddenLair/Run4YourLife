using UnityEngine;
using UnityEngine.UI;

using Run4YourLife.GameManagement;

namespace Run4YourLife.UI
{
    public class Progress : MonoBehaviour
    {
        [SerializeField]
        private Transform pointStart;

        [SerializeField]
        private Transform pointMiddle0;

        [SerializeField]
        private Transform pointMiddle1;

        [SerializeField]
        private Transform pointEnd;

        [SerializeField]
        private Sprite bossIconPhase1;

        [SerializeField]
        private Sprite bossIconPhase3;

        [SerializeField]
        private Image bossIcon;

        private Transform currentStart, currentEnd;

        void Awake()
        {
            SetPhase(GamePhase.EasyMoveHorizontal);
        }

        public void SetPhase(GamePhase gamePhase)
        {
            switch(gamePhase)
            {
                case GamePhase.EasyMoveHorizontal:
                    bossIcon.sprite = bossIconPhase1;
                    currentStart = pointStart;
                    currentEnd = pointMiddle0;
                    break;
                case GamePhase.HardMoveHorizontal:
                    bossIcon.sprite = bossIconPhase3;
                    currentStart = pointMiddle1;
                    currentEnd = pointEnd;
                    break;
            }

            SetPercent(0.0f);
        }

        public void SetPercent(float percent)
        {
            bossIcon.transform.position = Vector3.Lerp(currentStart.position, currentEnd.position, percent);
        }
    }
}