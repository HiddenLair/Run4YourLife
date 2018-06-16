using Run4YourLife.Utils;
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

    [SerializeField]
    private Transform activeGemPosition;

    private List<GameObject> activeGems = new List<GameObject>();

    public void ActivateGems(GameObject activatedGem)
    {
        unactiveGems.Remove(activatedGem);
        activeGems.Add(activatedGem);

        StartCoroutine(YieldHelper.WaitForSeconds(() => MoveGemToStand(activatedGem), 1));

        if (unactiveGems.Count == 0)
        {
            m_onAllGemsActive.Invoke();
        }
        else
        {
            StartCoroutine(YieldHelper.WaitForSeconds(()=>ActivateNextGem(), 2));
        }      
    }

    public void MoveGemToStand(GameObject activatedGem)
    {
        activatedGem.transform.position += new Vector3(0, 0, 1.75f);
    }

    public void ActivateNextGem()
    {
        GameObject activeGem = unactiveGems[0];
        activeGem.transform.position = activeGemPosition.position;

        //FLASHY APPEAR WITH SOUND NEEDED
        activeGem.SetActive(true);
    }
}
