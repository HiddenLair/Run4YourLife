using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SkillType { FIRE, WIND };

public class SkillControl : MonoBehaviour
{
    #region Public variables
    public SkillType skilltype;
    public int burningTime = 5;
    #endregion

    private void OnTriggerEnter(Collider collider)
    {
        switch(skilltype)
        {
            case SkillType.FIRE:
                ExecuteEvents.Execute<IEventMessageTarget>(collider.gameObject, null, (x, y) => x.Burned(burningTime));
                break;

            case SkillType.WIND:
                ExecuteEvents.Execute<IEventMessageTarget>(collider.gameObject, null, (x, y) => x.WindPush());
                break;
        }
    }
}
