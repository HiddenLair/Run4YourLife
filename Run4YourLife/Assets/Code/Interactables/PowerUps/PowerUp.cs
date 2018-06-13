using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

using Run4YourLife.GameManagement;
using Run4YourLife.Player;

namespace Run4YourLife.Interactables
{
    [RequireComponent(typeof(PlayableDirector))]
    public abstract class PowerUp : MonoBehaviour
    {
        protected enum PowerUpType { Void, Single, Shared };

        protected abstract PowerUpType Type { get; }

        private bool activated = false;
        private PlayableDirector m_playableDirector;

        public abstract void Apply(GameObject runner);

        protected virtual void Awake()
        {
            m_playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!activated && other.CompareTag(Tags.Runner))
            {
                activated = true;
                ExecutePowerUp(other.gameObject);
                StartCoroutine(PlayAndHide());
            }
        }

        private IEnumerator PlayAndHide()
        {
            m_playableDirector.Play();
            yield return new WaitUntil(() => m_playableDirector.state == PlayState.Paused);
            gameObject.SetActive(false);
        }

        private void ExecutePowerUp(GameObject runner)
        {
            switch (Type)
            {
                case PowerUpType.Void:
                    {
                        Apply(null);
                    }
                    break;
                case PowerUpType.Single:
                    {
                        Apply(runner);
                    }
                    break;
                case PowerUpType.Shared:
                    {
                        foreach (GameObject runnerGameObject in GameplayPlayerManager.Instance.RunnersAlive)
                        {
                            Apply(runnerGameObject);
                        }
                    }
                    break;
            }
        }
    }
}