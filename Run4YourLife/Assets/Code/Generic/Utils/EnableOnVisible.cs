using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Player;

public class EnableOnVisible : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_elementsToEnable;

    private void OnBecameVisible()
    {
        foreach (GameObject element in m_elementsToEnable)
        {
            element.GetComponent<ICustomVisibleInvisible>().OnCustomBecameVisible();
        }
    }

    private void OnBecameInvisible()
    {
        foreach (GameObject element in m_elementsToEnable)
        {
            element.GetComponent<ICustomVisibleInvisible>().OnCustomBecameInvisible();
        }
    }
}
