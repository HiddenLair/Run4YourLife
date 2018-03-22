using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BreakableItem : MonoBehaviour, IInteractableEvents {

    #region Public Variable
    public int hitsToBreak = 5;
    #endregion

    public void Interact()
    {
        hitsToBreak -= 1;
        
        if(hitsToBreak <= 0)
        {
            Destroy(gameObject);
        } 
    }
}
