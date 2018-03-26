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

        public float Get()
        {
            return walker.speed;
        }

        public void SetIncrease(float value)
        {
            walker.increaseValue = value;
            walker.increaseSpeedOverTime = value != 0.0f;
        }

        public void SetIncrease(string value)
        {
            float result;

            if(float.TryParse(value, out result))
            {
                SetIncrease(result);
            }
        }
    }
}