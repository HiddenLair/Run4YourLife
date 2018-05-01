using Run4YourLife;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Debug
{
    public class ForceGlobalManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_globalManagerPrefab;

        private void Awake()
        {
            GameObject globalManagerInstance = GameObject.FindGameObjectWithTag(Tags.GlobalManager);
            if (globalManagerInstance == null)
            {
                Instantiate(m_globalManagerPrefab);
            }
        }
    }
}
