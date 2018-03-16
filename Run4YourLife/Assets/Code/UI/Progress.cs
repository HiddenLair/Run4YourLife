using UnityEngine;
using UnityEngine.UI;

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

        // [SerializeField]
        // private Sprite bossIconPhase2;

        [SerializeField]
        private Sprite bossIconPhase3;

        [SerializeField]
        private Image bossIcon;

        private Vector3 currentStart, currentEnd;

        public void SetPhase(PhaseType phaseType)
        {
            switch(phaseType)
            {
                case PhaseType.FIRST:
                    bossIcon.sprite = bossIconPhase1;
                    currentStart = pointStart.position;
                    currentEnd = pointMiddle0.position;
                    break;
                /* case PhaseType.SECOND:
                    bossIcon.sprite = bossIconPhase2;
                    currentStart = pointMiddle0.position;
                    currentEnd = pointMiddle1.position;
                    break; */
                case PhaseType.THIRD:
                    bossIcon.sprite = bossIconPhase3;
                    currentStart = pointMiddle1.position;
                    currentEnd = pointEnd.position;
                    break;
            }

            SetPercent(0.0f);
        }

        public void SetPercent(float percent)
        {
            bossIcon.transform.position = Vector3.Lerp(currentStart, currentEnd, percent);
        }
    }
}