using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.InputManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Player {
    [RequireComponent(typeof(InputController))]
    [RequireComponent(typeof(PlayerInstance))]
    [RequireComponent(typeof(Collider))]
    public class RunnerRockThrowingController : MonoBehaviour {

        [SerializeField]
        private GameObject m_rockPrefab;

        [SerializeField]
        private Transform m_rockInstantiationTransform;

        [SerializeField]
        private float force;

        [SerializeField]
        private float angle;

        [SerializeField]
        private float Reload;

        [SerializeField]
        private AudioClip m_throwRockClip;

        private PlayerInstance m_playerInstance;
        private InputController m_inputController;
        private RunnerControlScheme m_runnerControlScheme;
        private new Collider collider;

        private Transform m_graphics;

        private float timer;

        private void Awake()
        {
            m_playerInstance = GetComponent<PlayerInstance>();
            m_inputController = GetComponent<InputController>();
            m_runnerControlScheme = GetComponent<RunnerControlScheme>();
            collider = GetComponent<Collider>();

            m_graphics = transform.Find("Graphics");
            Debug.Assert(m_graphics != null, "Graphics object not found in hierarchy");

            timer = Time.time;
        }

        private void Update()
        {
            if (m_inputController.Started(m_runnerControlScheme.Rock) && timer <= Time.time)
            {
                ThrowRock();
                timer = Time.time + Reload;
            }
        }

        private void ThrowRock()
        {
            AudioManager.Instance.PlaySFX(m_throwRockClip);
            GameObject rock = Instantiate(m_rockPrefab, m_rockInstantiationTransform.position, Quaternion.identity);
            Physics.IgnoreCollision(rock.GetComponent<Collider>(), collider);
            Rigidbody rigidbody = rock.GetComponent<Rigidbody>();
            rigidbody.velocity = GetThrowVelocity(force, angle);

            rock.GetComponent<RockController>().SetplayerHandle(m_playerInstance.PlayerHandle);
        }

        private Vector3 GetThrowVelocity(float force, float angle)
        {
            return GetQuaternionforFacingDirection(angle) * Vector3.right * force;
        }

        private Quaternion GetQuaternionforFacingDirection(float angle)
        {
            float facingDirectionAngle = m_graphics.eulerAngles.y == 90 ? 180 - angle : angle;
            return Quaternion.Euler(0, 0, facingDirectionAngle);
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 director = Quaternion.Euler(0, 0, angle) * Vector3.right;
            Gizmos.DrawLine(m_rockInstantiationTransform.position, m_rockInstantiationTransform.position + director*5);
        }
    }
}