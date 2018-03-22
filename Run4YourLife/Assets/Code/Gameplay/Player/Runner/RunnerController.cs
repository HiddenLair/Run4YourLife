using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Input;

public class RunnerController : MonoBehaviour {

    #region References

    private CharacterController m_characterController;
    private Stats m_stats;
    private RunnerControlScheme m_playerControlScheme;
    private Animator m_animator;
    private Animation m_currentAnimation;
    private AudioSource m_audioSource;

    #endregion

    #region Private Variables
    #endregion

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_playerControlScheme = GetComponent<RunnerControlScheme>();
        m_characterController = GetComponent<CharacterController>();
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
}
