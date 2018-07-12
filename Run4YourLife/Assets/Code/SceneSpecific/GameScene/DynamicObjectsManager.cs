using UnityEngine;
using Run4YourLife.Utils;

namespace Run4YourLife.GameManagement
{
    public class DynamicObjectsManager : SingletonMonoBehaviour<DynamicObjectsManager>
    {
        [SerializeField]
        private GameObjectPool m_gameObjectPool;

        public GameObjectPool GameObjectPool { get { return m_gameObjectPool; } }
    }
}