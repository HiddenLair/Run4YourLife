using System;
using System.Collections;
using System.Collections.Generic;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Utils;
using UnityEngine;

namespace Run4YourLife.Interactables
{
    public class TortoiseController : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve m_sinkAnimationCurve;

        [SerializeField]
        private float m_animationDuration;

        [SerializeField]
        private Animator m_animator;

        [SerializeField]
        private BoxCollider m_collider;

        [SerializeField]
        private AudioClip m_tortoiseSinkSFX;

        private Vector3 m_startingPosition;

        private bool m_isSinking;

        private bool IsIdle { get { return !m_isSinking; } }

        private bool RunnerOnTop
        {
            get
            {
                return Physics.CheckBox(m_collider.bounds.center, m_collider.bounds.extents, Quaternion.identity, Layers.Runner, QueryTriggerInteraction.Ignore);
            }
        }

        private void Awake()
        {
            m_startingPosition = transform.position;
        }

        private void OnDisable()
        {
            ResetState();
        }

        private void Update()
        {
            if (IsIdle && RunnerOnTop || Input.GetKeyDown(KeyCode.B))
            {
                StartCoroutine(SinkBehaviour());
            }
        }

        private IEnumerator SinkBehaviour()
        {
            m_isSinking = true;

            AudioManager.Instance.PlaySFX(m_tortoiseSinkSFX);
            m_animator.SetTrigger(TortoiseAnimator.Parameters.Triggers.Move);

            float animationTime;
            float endTime = Time.time + m_animationDuration;
            while (Time.time < endTime)
            {
                animationTime = 1f - ((endTime - Time.time) / m_animationDuration);
                transform.position = new Vector3()
                {
                    x = m_startingPosition.x,
                    y = m_startingPosition.y + m_sinkAnimationCurve.Evaluate(animationTime),
                    z = m_startingPosition.z
                };
                yield return null;
            }

            transform.position = m_startingPosition;

            m_isSinking = false;
        }


        private void ResetState()
        {
            m_isSinking = false;
            transform.position = m_startingPosition;
        }
    }



    public class TortoiseAnimator
    {
        public class States
        {
            public static readonly string Idle = "Idle";
            public static readonly string Moving = "Moving";
        }

        public class Parameters
        {
            public class Triggers
            {
                public static readonly string Move = "Move";
            }
        }
    }
}
