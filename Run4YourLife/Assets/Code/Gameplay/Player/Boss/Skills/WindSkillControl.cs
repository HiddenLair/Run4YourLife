using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindSkillControl : MonoBehaviour {

    #region Enumerators

    public enum Direction { LEFT,RIGHT};

    #endregion

    #region Inspector
    [SerializeField]
    private float timeToDie = 5;

    [SerializeField]
    private Direction direction; 

    #endregion

    #region Variables
    HashSet<GameObject> colliders = new HashSet<GameObject>();
    #endregion

    private void Awake()
    {
        Destroy(gameObject, timeToDie);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.Player))
        {
            colliders.Add(collider.gameObject);
            if (direction == Direction.LEFT)
            {
                ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.ActivateWindLeft());
            }
            else
            {
                ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.ActivateWindRight());
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag(Tags.Player))
        {
            colliders.Remove(collider.gameObject);
            if (direction == Direction.LEFT)
            {
                ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.DeactivateWindLeft());
            }
            else
            {
                ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.DeactivateWindRight());
            }
        }
    }

    private void OnDestroy()
    {
        foreach(GameObject gO in colliders)
        {
            if (direction == Direction.LEFT)
            {
                ExecuteEvents.Execute<ICharacterEvents>(gO, null, (x, y) => x.DeactivateWindLeft());
            }
            else
            {
                ExecuteEvents.Execute<ICharacterEvents>(gO, null, (x, y) => x.DeactivateWindRight());
            }
        }
    }
}
