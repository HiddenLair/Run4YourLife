﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindSkillControl : MonoBehaviour {

    #region Inspector

    [SerializeField]
    private float timeToDie = 5;

    [SerializeField]
    private float windForce;

    #endregion

    #region Variables

    HashSet<GameObject> m_objectsInside = new HashSet<GameObject>();

    #endregion

    private void Awake()
    {
        Destroy(gameObject, timeToDie);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.Player))
        {
            m_objectsInside.Add(collider.gameObject);
            ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.ActivateWind(windForce));
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag(Tags.Player))
        {
            m_objectsInside.Remove(collider.gameObject);
            ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.DeactivateWind(windForce));
        }
    }

    private void OnDestroy()
    {
        foreach(GameObject objectInside in m_objectsInside)
        {
            ExecuteEvents.Execute<ICharacterEvents>(objectInside, null, (x, y) => x.DeactivateWind(windForce));
        }
    }
}
