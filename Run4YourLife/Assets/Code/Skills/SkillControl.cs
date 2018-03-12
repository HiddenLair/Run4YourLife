using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SkillType { FIRE };

public class SkillControl : MonoBehaviour
{
    #region Public variables
    public SkillType skilltype;
    #endregion

    #region Private variables
    private float timer = 0.0f;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        switch(skilltype)
        {
            case SkillType.FIRE:
                ExecuteEvents.Execute<IEventMessageTarget>(collider.gameObject, null, (x, y) => x.Burned());
                break;
        }
    }
}
