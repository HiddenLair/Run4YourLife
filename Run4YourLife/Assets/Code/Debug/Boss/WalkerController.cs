using UnityEngine;

namespace Run4YourLife.DebuggingTools
{
    public class WalkerController : MonoBehaviour
    {
        private Walker walker = null;

        private GameObject boss = null;

        void Update()
        {
            if(boss == null)
            {
                boss = GameObject.FindGameObjectWithTag("Boss");

                if(boss)
                {
                    walker = boss.GetComponent<Walker>();
                }
            }
        }

        public bool Exists()
        {
            return walker != null;
        }

        public float GetSpeed()
        {
            return walker.speed;
        }

        public void SetSpeed(float value)
        {
            walker.speed = value;
        }

        public void SetSpeed(string value)
        {
            float result;

            if(float.TryParse(value, out result))
            {
                SetSpeed(result);
            }
        }

        public float GetIncreaseSpeed()
        {
            return walker.increaseValue;
        }

        public void SetIncreaseSpeed(float value)
        {
            walker.increaseValue = value;
            walker.increaseSpeedOverTime = value != 0.0f;
        }

        public void SetIncreaseSpeed(string value)
        {
            float result;

            if(float.TryParse(value, out result))
            {
                SetIncreaseSpeed(result);
            }
        }
    }
}