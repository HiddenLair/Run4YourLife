using UnityEngine;
using Run4YourLife.Utils;

namespace Run4YourLife.GameManagement
{
    [RequireComponent(typeof(GameObjectPool))]
    [RequireComponent(typeof(ManualPrefabInstantiator))]
    public class BossPoolManager : SingletonMonoBehaviour<BossPoolManager>
    {
        [SerializeField]
        private int m_objectRepetitions;

        [SerializeField]
        private GameObject[] m_objectsForPool;

        [SerializeField]
        private GameObject m_pool;

        private GameObjectPool m_gameObjectPool;
        private ManualPrefabInstantiator m_manualPrefabInstantiator;

        private void Awake()
        {
            m_gameObjectPool = GetComponent<GameObjectPool>();
            m_manualPrefabInstantiator = GetComponent<ManualPrefabInstantiator>();
        }

        void Start()
        {
            foreach(GameObject g in m_objectsForPool)
            {
                m_gameObjectPool.Add(g, m_objectRepetitions);
            }
        }

        public GameObject InstantiateBossElement(GameObject g, Vector3 instancePosition, bool activate=true)
        {
           return m_manualPrefabInstantiator.ManualInstantiate(g, instancePosition, activate);
        }
    }
}