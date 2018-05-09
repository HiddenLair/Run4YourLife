using UnityEngine;
using Run4YourLife.UI;
using Run4YourLife.InputManagement;
using UnityEngine.EventSystems;

namespace Run4YourLife.Player
{
    [RequireComponent(typeof(Ready))]
    [RequireComponent(typeof(BossControlScheme))]
    public abstract class Melee : MonoBehaviour
    {
        #region Editor variables

        [SerializeField]
        [Range(0, 1)]
        private float triggerSensivility = 0.2f;

        [SerializeField]
        private float reloadTimeS;

        [SerializeField]
        protected AudioClip sfx;

        #endregion

        private float currentTimeS = 0;
        private bool alreadyPressed = false;

        private Ready ready;
        private BossControlScheme controlScheme;
        
        private GameObject uiManager;

        protected virtual void Awake()
        {
            currentTimeS = reloadTimeS;

            ready = GetComponent<Ready>();
            controlScheme = GetComponent<BossControlScheme>();

            uiManager = GameObject.FindGameObjectWithTag(Tags.UI);
        }

        private void Start()
        {
            controlScheme.Active = true;
        }

        void Update()
        {
            currentTimeS = Mathf.Min(currentTimeS + Time.deltaTime, reloadTimeS);

            Verify();
        }

        private void Verify()
        {
            if(ready.Get())
            {
                if(controlScheme.Melee.Value() > triggerSensivility)
                {
                    if(currentTimeS >= reloadTimeS && !alreadyPressed)
                    {
                        OnSuccess();

                        currentTimeS = 0.0f;
                        alreadyPressed = true;

                        ExecuteEvents.Execute<IUIEvents>(uiManager, null, (x, y) => x.OnActionUsed(ActionType.MELE, reloadTimeS));
                    }
                }
                else
                {
                    alreadyPressed = false;
                }
            }
        }

        protected abstract void OnSuccess();
    }
}