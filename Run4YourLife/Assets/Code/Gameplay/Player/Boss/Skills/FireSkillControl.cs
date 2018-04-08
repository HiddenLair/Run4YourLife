using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireSkillControl : MonoBehaviour {

    #region Public variables
    public int burningTime = 5;
    public int timeToDie = 5;
    #endregion

    private void Awake()
    {
        Destroy(gameObject, timeToDie);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.Runner))
        {
            ExecuteEvents.Execute<ICharacterEvents>(collider.gameObject, null, (x, y) => x.Burned(burningTime));
        }
    }
}
