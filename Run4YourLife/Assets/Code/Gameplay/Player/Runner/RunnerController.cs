using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Input;
using Run4YourLife.Player;
using Run4YourLife.GameManagement;

public class RunnerController : MonoBehaviour, ICharacterEvents
{

    #region References

    private Stats m_stats;
    private RunnerControlScheme m_playerControlScheme;
    private RunnerCharacterController m_runnerCharacterController;
    private Animator m_animator;

    #endregion

    private void Awake()
    {
        m_playerControlScheme = GetComponent<RunnerControlScheme>();
        m_runnerCharacterController = GetComponent<RunnerCharacterController>();
        m_stats = GetComponent<Stats>();
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        m_playerControlScheme.Active = true;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag(Tags.Interactable) && m_playerControlScheme.interact.Started())
        {
            Interact(collider.gameObject);
        }
    }

    private void Interact(GameObject gameObject)
    {
        ExecuteEvents.Execute<IInteractableEvents>(gameObject, null, (x, y) => x.Interact());
    }

    public void Kill()
    {
        GameObject playerStateManager = FindObjectOfType<PlayerStateManager>().gameObject;
        PlayerDefinition playerDefinition = GetComponent<PlayerInstance>().PlayerDefinition;
        ExecuteEvents.Execute<IPlayerStateEvents>(playerStateManager, null, (x, y) => x.OnPlayerDeath(playerDefinition));
        Destroy(gameObject);
    }

    public void Impulse(Vector3 direction, float force)
    {
        m_runnerCharacterController.Impulse(direction, force);
    }

    #region Root

    public void Root(int rootHardness)
    {
        StartCoroutine(RootCoroutine(rootHardness));
    }

    private IEnumerator RootCoroutine(int rootHardness)
    {
        m_animator.SetTrigger("root");
        m_animator.SetFloat("xSpeed", 0.0f);

        m_stats.root = true;
        m_stats.rootHardness = rootHardness;

        m_playerControlScheme.Active = false;
        m_playerControlScheme.interact.enabled = true;

        while (m_stats.root)
        {
            yield return null;

            if (m_playerControlScheme.interact.Started())
            {
                m_stats.rootHardness -= 1;
            }

            m_stats.root = m_stats.rootHardness > 0;
        }
        m_playerControlScheme.Active = true;
    }

    #endregion

    public void Debuff(StatModifier statmodifier)
    {
        m_stats.AddModifier(statmodifier);
    }

    #region Burned

    public void Burned(int burnedTime)
    {
        if (!m_stats.burned)
        {
            StartCoroutine(BurnedCoroutine(burnedTime));
        }
    }

    private IEnumerator BurnedCoroutine(int burnedTime)
    {
        m_stats.burned = true;
        yield return new WaitForSeconds(burnedTime);
        m_stats.burned = false;
    }

    #endregion

    #region WindPush

    public void ActivateWindPush()
    {
        m_stats.windPush = true;
    }

    public void DeactivateWindPush()
    {
        m_stats.windPush = false;
    }

    #endregion
}
