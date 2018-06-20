using System.Collections.Generic;

namespace Run4YourLife
{
    public class GlobalDataContainer : SingletonMonoBehaviour<GlobalDataContainer> {

        private Dictionary<string, object> m_data = new Dictionary<string, object>();

        public bool Has(string key)
        {
            return m_data.ContainsKey(key);
        }

        public object Get(string key)
        {
            object data = null;
            m_data.TryGetValue(key, out data);

            return data;
        }

        public void Set(string key, object data)
        {
            if(Has(key))
            {
                m_data[key] = data;
            }
            else
            {
                m_data.Add(key, data);
            }
        }
    }
}