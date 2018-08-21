using System.Collections;

using UnityEngine;

using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.GameManagement;
using Run4YourLife.Interactables;
using Run4YourLife.Player.Runner;
using System;

namespace Run4YourLife.Player.Boss.Skills.Bomb
{
    [RequireComponent(typeof(SimulateChildOf))]
    [RequireComponent(typeof(Rigidbody))]
    public class BombController : SkillBase, IBossSkillBreakable
    {
        #region Inspector

        [SerializeField]
        private float m_fadeInTime;

        [SerializeField]
        private float gravity;

        [SerializeField]
        private float initialSpeed;

        [SerializeField]
        private float m_explosionRatius;

        [SerializeField]
        private float timeBetweenActions;

        [SerializeField]
        private float jumpHeight;

        [SerializeField]
        private float fireGrowDuration;

        [SerializeField]
        private float fireStableDuration;

        [SerializeField]
        private FireController fireScript;

        [SerializeField]
        private Collider m_runnerDetectorTrigger;

        [SerializeField]
        private Collider m_skillBreakTrigger;

        [SerializeField]
        private Collider bossSpawnCheckCollider;

        [SerializeField]
        private FXReceiver spawnPortalParticles;

        [SerializeField]
        private FXReceiver explosionParticles;

        [SerializeField]
        private FXReceiver jumpReceiver;

        [SerializeField]
        TrembleConfig trembleFall;

        [SerializeField]
        TrembleConfig trembleExplosion;

        [SerializeField]
        protected AudioClip m_trapfallClip;

        [SerializeField]
        protected AudioClip m_trapDetonationClip;

        [SerializeField]
        private AudioClip m_fireClip;

        #endregion

        #region Variables

        private Vector3 speed;

        private Renderer m_renderer;
        private SimulateChildOf simulateChildOf;
        private new Rigidbody rigidbody;

        #endregion

        private void Awake()
        {
            m_renderer = GetComponentInChildren<Renderer>();
            Debug.Assert(m_renderer != null);

            simulateChildOf = GetComponent<SimulateChildOf>();
            rigidbody = GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public override bool CheckAndRepositionSkillSpawn(ref SkillSpawnData skillSpawnData)
        {
            Collider[] colliders = Physics.OverlapBox(skillSpawnData.position, bossSpawnCheckCollider.bounds.extents, Quaternion.identity, Layers.Stage, QueryTriggerInteraction.Ignore);
            if (colliders.Length != 0)
            {
                return false;
            }
            return true;
        }

        protected override void ResetState()
        {
            StopAllCoroutines();

            simulateChildOf.Parent = null;

            m_runnerDetectorTrigger.enabled = false;
            m_skillBreakTrigger.enabled = false;

            speed.y = initialSpeed;

            SetRandomNoise(m_renderer.material);

            if (fireScript != null)
            {
                fireScript.Stop();
            }
        }

        private void SetRandomNoise(Material mat)
        {
            if (mat.HasProperty("_Noise"))
            {
                mat.SetTextureOffset("_Noise", new Vector2()
                {
                    x = Mathf.Sin(Time.time),
                    y = Mathf.Cos(Time.time)
                });
            }
        }

        protected override void OnSkillStart()
        {
            spawnPortalParticles.PlayFx(false);

            StartCoroutine(BombStartingBehaviour());
        }

        private IEnumerator BombStartingBehaviour()
        {
            yield return StartCoroutine(FadeInBomb());

            //Enable collisions
            m_runnerDetectorTrigger.enabled = true;
            m_skillBreakTrigger.enabled = true;

            yield return StartCoroutine(Fall());

            //Start Behaviour for the phase
            switch (phase)
            {
                case SkillBase.Phase.PHASE2:
                    StartCoroutine(Phase2BombGroundedBehaviour());
                    break;
                case SkillBase.Phase.PHASE3:
                    StartCoroutine(Phase3BombGroundedBehaviour());
                    break;
            }
        }

        private IEnumerator FadeInBomb()
        {
            float endTime = Time.time + m_fadeInTime;
            while (Time.time < endTime)
            {
                if (m_renderer.material.HasProperty("_Dissolveamout"))
                {
                    float dissolve = m_renderer.material.GetFloat("_Dissolveamout");
                    dissolve = (endTime - Time.time) / m_fadeInTime;
                    m_renderer.material.SetFloat("_Dissolveamout", dissolve);
                }
                yield return null;
            }
        }

        private IEnumerator Fall()
        {
            RaycastHit raycastHit;
            Vector3 movement = speed * Time.deltaTime;
            while (!rigidbody.SweepTest(movement, out raycastHit, movement.magnitude, QueryTriggerInteraction.Ignore))
            {
                transform.Translate(movement);
                yield return null;
                speed.y += gravity * Time.deltaTime;
                movement = speed * Time.deltaTime;
            }

            transform.position = raycastHit.point;
            simulateChildOf.Parent = raycastHit.transform;

            TrembleManager.Instance.Tremble(trembleFall);
            AudioManager.Instance.PlaySFX(m_trapfallClip);
        }

        private IEnumerator Phase2BombGroundedBehaviour()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeBetweenActions);
                PlayFire();
            }
        }

        private IEnumerator Phase3BombGroundedBehaviour()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeBetweenActions);

                PlayFire();
                yield return StartCoroutine(Jump());
            }
        }

        private void PlayFire()
        {
            fireScript.Play(fireGrowDuration, fireStableDuration);
            AudioManager.Instance.PlaySFX(m_fireClip);
        }

        private IEnumerator Jump()
        {
            jumpReceiver.PlayFx();

            speed.y = HeightToVelocity(jumpHeight);
            while (speed.y > 0)
            {
                transform.Translate(speed * Time.deltaTime);
                speed.y += gravity * Time.deltaTime;
                yield return null;
            }

            yield return StartCoroutine(Fall());
        }

        private float HeightToVelocity(float height)
        {
            return Mathf.Sqrt(height * -2.0f * gravity);
        }

        public void Explode()
        {
            StopAllCoroutines();

            Collider[] collisions = Physics.OverlapSphere(transform.position, m_explosionRatius, Layers.Runner);

            foreach (Collider c in collisions)
            {
                if (!Physics.Linecast(transform.position, c.bounds.center, Layers.Stage))
                {
                    IRunnerEvents runnerEvents = c.GetComponent<IRunnerEvents>();
                    if (runnerEvents != null)
                    {
                        runnerEvents.Kill();
                    }
                }
            }

            AudioManager.Instance.PlaySFX(m_trapDetonationClip);
            TrembleManager.Instance.Tremble(trembleExplosion);
            explosionParticles.PlayFx(false);
            gameObject.SetActive(false);
        }

        void IBossSkillBreakable.Break()
        {
            Explode();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_explosionRatius);
        }
    }
}