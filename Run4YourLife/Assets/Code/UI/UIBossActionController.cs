using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Run4YourLife.UI
{
    public class UIBossActionController : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector m_usePlayableDirector;

        [SerializeField]
        private Image m_cooldownImage;

        [SerializeField]
        private UnityEvent onUseStart;

        [SerializeField]
        private UnityEvent onUseEnd;

        [SerializeField]
        private float minFillAmount = 0.0f;

        [SerializeField]
        private float maxFillAmount = 1.0f;

        private Coroutine useCoroutine;

        public void Use(float cooldown)
        {
            Reset();

            useCoroutine = StartCoroutine(UseCoroutine(cooldown));
        }

        public void Reset()
        {
            if(useCoroutine != null)
            {
                StopCoroutine(useCoroutine);
                useCoroutine = null;
                ResetUseState();
            }
        }

        private void ResetUseState()
        {
            m_cooldownImage.fillAmount = ComputeFillAmount(0.0f);
        }

        private IEnumerator UseCoroutine(float cooldown)
        {
            onUseStart.Invoke();

            float startTime = Time.time;
            float endTime = startTime + cooldown;

            m_cooldownImage.fillAmount = ComputeFillAmount(1.0f);
            m_usePlayableDirector.Play();
            while(Time.time < endTime)
            {
                m_cooldownImage.fillAmount = ComputeFillAmount((endTime - Time.time) / cooldown); 
                yield return null;
            }

            m_cooldownImage.fillAmount = ComputeFillAmount(0.0f);

            onUseEnd.Invoke();
        }

        private float ComputeFillAmount(float value)
        {
            return Mathf.Lerp(minFillAmount, maxFillAmount, value);
        }
    }
}