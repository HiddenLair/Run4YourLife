using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife
{
    public class GlobalDataContainer : SingletonMonoBehaviour<GlobalDataContainer> {

        private Dictionary<string, object> m_data = new Dictionary<string, object>();

        public Dictionary<string,object> Data { get { return m_data; } }  
    }
}

