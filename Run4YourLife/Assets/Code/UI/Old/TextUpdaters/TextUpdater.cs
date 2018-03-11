/* using UnityEngine;
using UnityEngine.UI;

namespace Run4YourLife.UI
{
    public abstract class TextUpdater : Updater
    {
        private Text text = null;

        protected GetRemainingTimeDelegate getRemainingTimeDelegate = null;

        void Start()
        {
            text = GetComponent<Text>();
        }

        void Update()
        {
            Debug.Assert(canDoActionDelegate != null);
            Debug.Assert(getRemainingTimeDelegate != null);

            float remainingTime = getRemainingTimeDelegate();

            if(remainingTime > 0.0f)
            {
                text.text = remainingTime.ToString("0.###");
            }
            else if(remainingTime == 0.0f && !canDoActionDelegate())
            {
                text.text = "DISABLED";
            }
            else
            {
                text.text = "ENABLED";
            }
        }

        protected delegate float GetRemainingTimeDelegate();
    }
} */