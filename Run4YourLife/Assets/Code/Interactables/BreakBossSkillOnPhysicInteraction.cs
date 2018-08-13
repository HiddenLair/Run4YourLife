using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Interactables;

public class BreakBossSkillOnPhysicInteraction : MonoBehaviour, IBossSkillBreakable
{
    [SerializeField]
    private GameObject m_bossSkillBreakable;

    private void OnTriggerEnter(Collider collider)
    {
        if (m_bossSkillBreakable.activeSelf)
        {
            CheckBossSkillBreakableAndSendBreakEvent(collider.gameObject);
        }
    }

    private void CheckBossSkillBreakableAndSendBreakEvent(GameObject gameObject)
    {
        IBossSkillBreakable bossSkillBreakable = gameObject.GetComponent<IBossSkillBreakable>();
        if (bossSkillBreakable != null)
        {
            m_bossSkillBreakable.GetComponent<IBossSkillBreakable>().Break();
            bossSkillBreakable.Break();
        }
    }

    private void OnValidate()
    {
        Debug.Assert(m_bossSkillBreakable != null);
        Debug.Assert(m_bossSkillBreakable.GetComponent<IBossSkillBreakable>() != null);
    }

    public void Break()
    {
        m_bossSkillBreakable.GetComponent<IBossSkillBreakable>().Break();
    }
}
