using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public abstract class MenuEntryArrowed : MonoBehaviour, IMoveHandler, ISelectHandler, IDeselectHandler
    {
        protected enum MoveEvent { Left = -1, Right = 1 }

        [SerializeField]
        private AudioClip m_switchClip;

        [SerializeField]
        private GameObject m_leftArrow;

        [SerializeField]
        private GameObject m_rightArrow;

        [SerializeField]
        private Sprite arrowNotSelected;

        [SerializeField]
        private Sprite arrowSelected;

        private Image leftArrowImage;
        private Image rightArrowImage;

        private PlayableDirector m_leftArrowPlayableDirector;
        private PlayableDirector m_rightArrowPlayableDirector;

        protected virtual void Awake()
        {
            leftArrowImage = m_leftArrow.GetComponent<Image>();
            Debug.Assert(leftArrowImage != null);

            rightArrowImage = m_rightArrow.GetComponent<Image>();
            Debug.Assert(rightArrowImage != null);

            m_leftArrowPlayableDirector = m_leftArrow.GetComponent<PlayableDirector>();
            Debug.Assert(m_leftArrowPlayableDirector != null);

            m_rightArrowPlayableDirector = m_rightArrow.GetComponent<PlayableDirector>();
            Debug.Assert(m_rightArrowPlayableDirector != null);

            leftArrowImage.sprite = arrowNotSelected;
            rightArrowImage.sprite = arrowNotSelected;
        }

        public void OnMove(AxisEventData eventData)
        {
            bool playSFX = false;

            switch (eventData.moveDir)
            {
                case MoveDirection.Right:
                    m_rightArrowPlayableDirector.Play();
                    OnArrowEvent(MoveEvent.Right);
                    playSFX = true;
                    break;
                case MoveDirection.Left:
                    m_leftArrowPlayableDirector.Play();
                    OnArrowEvent(MoveEvent.Left);
                    playSFX = true;
                    break;
            }

            if(playSFX && m_switchClip != null)
            {
                AudioManager.Instance.PlaySFX(m_switchClip);
            }
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            leftArrowImage.sprite = arrowSelected;
            rightArrowImage.sprite = arrowSelected;
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            leftArrowImage.sprite = arrowNotSelected;
            rightArrowImage.sprite = arrowNotSelected;
        }

        protected abstract void OnArrowEvent(MoveEvent moveEvent);
    }
}