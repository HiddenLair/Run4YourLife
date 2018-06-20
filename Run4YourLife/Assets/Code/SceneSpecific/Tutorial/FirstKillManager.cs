using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.GameManagement
{
    public class FirstKillManager : SingletonMonoBehaviour<FirstKillManager>
    {

        [SerializeField]
        private GameObject reviveText;

        [SerializeField]
        private float minTimeToWatch;

        private bool doUpdate = false;
        private float timer = 0.0f;

        public void ShowReviveInfo()
        {
            reviveText.SetActive(true);
            Time.timeScale = 0.0f;
            timer = Time.time + minTimeToWatch;
            doUpdate = true;
        }

        private void Update()
        {
            if (doUpdate)
            {
                if(timer < Time.time)
                {
                    if (Input.anyKeyDown)
                    {
                        Time.timeScale = 1.0f;
                        reviveText.SetActive(false);
                        doUpdate = false;
                    }
                }
            }
        }
    }
}
