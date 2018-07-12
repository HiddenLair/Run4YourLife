using UnityEngine;
using Run4YourLife.Utils;

namespace Run4YourLife.GameManagement
{
    public class DynamicObjectsManager : SingletonMonoBehaviour<DynamicObjectsManager>
    {
        [SerializeField]
        private GameObjectPool m_gameObjectPool;

        public GameObjectPool GameObjectPool { get { return m_gameObjectPool; } }

        private void OnValidate()
        {
            Debug.Assert(m_gameObjectPool != null);
        }

        //TODO: REMOVE & REFACTOR BELOW THIS. EACH SKILL SHOULD BE RESPONSIBLE FOR POOLING IT'S OWN STUFF NOT A GLOBAL MANAGER
        [SerializeField]
        private int m_objectRepetitions;

        [SerializeField]
        private GameObject[] m_objectsForPool;

        void Start()
        {
            foreach (GameObject g in m_objectsForPool)
            {
                m_gameObjectPool.Add(g, m_objectRepetitions);
            }
        }
    }
}