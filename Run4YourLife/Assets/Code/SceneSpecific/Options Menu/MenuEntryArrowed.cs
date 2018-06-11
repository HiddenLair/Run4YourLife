using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public abstract class MenuEntryArrowed : MonoBehaviour, IMoveHandler, ISelectHandler, IDeselectHandler
    {

        protected enum MoveEvent
            {
                Left,
                Right
            }

        [SerializeField]
        private AudioClip m_switchClip;

        [SerializeField]
        private GameObject m_leftArrow;

        [SerializeField]
        private GameObject m_rightArrow;

        private PlayableDirector m_leftArrowPlayableDirector;
        private PlayableDirector m_rightArrowPlayableDirector;

        protected virtual void Awake()
        {
            m_leftArrowPlayableDirector = m_leftArrow.GetComponent<PlayableDirector>();
            Debug.Assert(m_leftArrowPlayableDirector != null);

            m_rightArrowPlayableDirector = m_rightArrow.GetComponent<PlayableDirector>();
            Debug.Assert(m_rightArrowPlayableDirector != null);

            m_leftArrow.SetActive(false);
            m_rightArrow.SetActive(false);
        }

        public void OnMove(AxisEventData eventData)
        {
            switch(eventData.moveDir)
            {
                case MoveDirection.Right:
                    m_rightArrowPlayableDirector.Play();
                    OnArrowEvent(MoveEvent.Right);
                    break;
                case MoveDirection.Left:
                    m_leftArrowPlayableDirector.Play();
                    OnArrowEvent(MoveEvent.Left);
                    break;
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            m_leftArrow.SetActive(true);
            m_rightArrow.SetActive(true);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            m_leftArrow.SetActive(false);
            m_rightArrow.SetActive(false);
            AudioManager.Instance.PlaySFX(m_switchClip);
        }

        protected abstract void OnArrowEvent(MoveEvent moveEvent);
    }
}
