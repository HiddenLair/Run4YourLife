using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Interactables;

public class BreakBossSkillOnPhysicInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject m_bossSkillBreakable;

    private void OnTriggerEnter(Collider collider)
    {
        CheckBossSkillBreakableAndSendBreakEvent(collider.gameObject);
    }

    private void CheckBossSkillBreakableAndSendBreakEvent(GameObject runner)
    {
        IBossSkillBreakable bossSkillBreakable = runner.GetComponent<IBossSkillBreakable>();
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
}
