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
                            PlayerHandle player = other.GetComponent<PlayerInstance>().PlayerHandle;
                            ExecuteEvents.Execute<IScoreEvents>(ScoreManager.InstanceGameObject, null, (x, y) => x.OnAddPoints(player, points));
                        }
                        break;
                    case Type.GROUP:
                        {
                            foreach (GameObject runner in GameplayPlayerManager.Instance.RunnersAlive)
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