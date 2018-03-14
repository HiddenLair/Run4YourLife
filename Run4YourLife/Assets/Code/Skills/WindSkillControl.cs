using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindSkillControl : MonoBehaviour {

    #region Public Variables
    public int timeToDie = 5;
    #endregion

    #region Private Variables
    HashSet<GameObject> colliders = new HashSet<GameObject>();
    #endregion

    private void Awake()
    {
        Destroy(gameObject, timeToDie);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            colliders.Add(collider.gameObject);
            ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.ActivateWindPush());
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            colliders.Remove(collider.gameObject);
            ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.DeactivateWindPush());
        }
    }

    private void OnDestroy()
    {
        foreach(GameObject gO in colliders)
        {
            ExecuteEvents.Execute<ICharacterEvents>(gO, null, (x, y) => x.DeactivateWindPush());
        }
    }
}
