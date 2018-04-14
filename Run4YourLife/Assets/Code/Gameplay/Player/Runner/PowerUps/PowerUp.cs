using UnityEngine;

using Run4YourLife.GameManagement;

namespace Run4YourLife.Player
{
    public abstract class PowerUp : MonoBehaviour
    {
        public enum Type { SINGLE, GROUP };

        [SerializeField]
        private Type m_type;

        public abstract void Apply(GameObject runner);

        private void OnTriggerEnter(Collider other)
        {
            switch (m_type)
            {
                case Type.SINGLE:
                    {
                        Debug.Assert(other.CompareTag(Tags.Runner));
                        Apply(other.gameObject);
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