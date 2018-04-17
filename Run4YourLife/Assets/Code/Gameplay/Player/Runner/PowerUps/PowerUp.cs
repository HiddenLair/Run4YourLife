using UnityEngine;
using UnityEngine.EventSystems;
using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    public abstract class PowerUp : MonoBehaviour
    {
        public enum Type { SINGLE, GROUP };

        [SerializeField]
        private Type m_type;

        [SerializeField]
        private float points;

        public abstract void Apply(GameObject runner);

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(Tags.Runner))
            {
                switch (m_type)
                {
                    case Type.SINGLE:
                        {
                            Debug.Assert(other.CompareTag(Tags.Runner));
                            Apply(other.gameObject);
                            PlayerDefinition player = other.gameObject.GetComponent<PlayerInstance>().PlayerDefinition;
                            ExecuteEvents.Execute<IScoreEvents>(FindObjectOfType<ScoreManager>().gameObject, null, (x, y) => x.OnAddPoints(player, points));
                        }
                        break;
                    case Type.GROUP:
                        {
                            GameplayPlayerManager gameplayPlayerManager = FindObjectOfType<GameplayPlayerManager>();
                            Debug.Assert(gameplayPlayerManager != null);

                            foreach (GameObject runner in gameplayPlayerManager.RunnersAlive)
                            {
                                Apply(runner);
                            }
                        }
                        break;
                }
                Destroy(gameObject);
            }
        }
    }
}