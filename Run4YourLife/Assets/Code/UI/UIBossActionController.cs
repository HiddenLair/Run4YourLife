using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

namespace Run4YourLife.UI
{
    public class UIBossActionController : MonoBehaviour {

        [SerializeField]
        private PlayableDirector m_usePlayableDirector;

        [SerializeField]
        private Image m_cooldownImage;

        private Coroutine useCoroutine;

        public void Use(float cooldown)
        {
            if(useCoroutine != null)
            {
                StopCoroutine(useCoroutine);
                useCoroutine = null;
                ResetUseState();
            }
            useCoroutine = StartCoroutine(UseCoroutine(cooldown));
        }

        private void ResetUseState()
        {
            m_cooldownImage.fillAmount = 0; 
        }

        private IEnumerator UseCoroutine(float cooldown)
        {
            float startTime = Time.time;
            float endTime = startTime + cooldown;
            
            m_cooldownImage.fillAmount = 1; 
            m_usePlayableDirector.Play();
            while(Time.time < endTime)
            {
                m_cooldownImage.fillAmount = (endTime - Time.time) / cooldown; 
                yield return null;
            }

            m_cooldownImage.fillAmount = 0; 
        }
    }
}
