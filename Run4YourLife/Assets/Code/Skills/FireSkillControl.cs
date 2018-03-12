using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireSkillControl : MonoBehaviour {

    #region Public variables
    public int burningTime = 5;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player")
        ExecuteEvents.Execute<IEventMessageTarget>(collider.gameObject, null, (x, y) => x.Burned(burningTime));     
    }
}
