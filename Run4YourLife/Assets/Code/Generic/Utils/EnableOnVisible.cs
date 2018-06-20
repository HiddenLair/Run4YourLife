using UnityEngine;
using Run4YourLife.Player;

namespace Run4YourLife.Utils
{
    public class EnableOnVisible : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_elementsToEnable;

        private void OnBecameVisible()
        {
            foreach(GameObject element in m_elementsToEnable)
            {
                element.GetComponent<ICustomVisibleInvisible>().OnCustomBecameVisible();
            }
        }

        private void OnBecameInvisible()
        {
            foreach(GameObject element in m_elementsToEnable)
            {
                element.GetComponent<ICustomVisibleInvisible>().OnCustomBecameInvisible();
            }
        }
    }
}