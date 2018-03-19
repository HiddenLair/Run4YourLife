using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BreakableItem : MonoBehaviour, IPropEvents {

    #region Public Variable
    public int hitsToBreak = 5;
    #endregion

    public void OnInteraction()
    {
        hitsToBreak -= 1;
        
        if(hitsToBreak <= 0)
        {
            Destroy(gameObject);
        } 
    }
}
