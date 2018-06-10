using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossFightGemManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_onAllGemsActive;

    [SerializeField]
    private List<GameObject> unactiveGems;

    private List<GameObject> activeGems = new List<GameObject>();

    public void ActivateGems(GameObject gemToActivate)
    {
        unactiveGems.Remove(gemToActivate);
        activeGems.Add(gemToActivate);

        if(unactiveGems.Count == 0)
        {
            m_onAllGemsActive.Invoke();
        }
        else
        {
            ActivateNextGem();
        }
    }

    private void ActivateNextGem()
    {
        unactiveGems[0].SetActive(true);
    }
}
