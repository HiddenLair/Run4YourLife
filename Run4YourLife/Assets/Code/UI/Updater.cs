using UnityEngine;

using Run4YourLife.Player;

namespace Run4YourLife.UI
{
    public abstract class Updater : MonoBehaviour
    {
        [SerializeField]
        protected Boss boss = null;

        [SerializeField]
        protected Laser laser = null;

        protected CanDoActionDelegate canDoActionDelegate = null;

        void Awake()
        {
            Configure();
        }

        protected abstract void Configure();

        protected delegate bool CanDoActionDelegate();
    }
}