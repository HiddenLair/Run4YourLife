using System.Collections;

using UnityEngine;

using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.GameManagement;
using Run4YourLife.Interactables;
using Run4YourLife.Player.Runner;

namespace Run4YourLife.Player.Boss.Skills.Bomb
{
    public class BombController : SkillBase, IBossSkillBreakable
    {
        #region Inspector

        [SerializeField]
        protected AudioClip m_trapfallClip;

        [SerializeField]
        protected AudioClip m_trapDetonationClip;

        [SerializeField]
        private float m_fadeInTime;

        [SerializeField]
        private float rayCheckerLenght = 10.0f;

        [SerializeField]
        private float gravity = -9.8f;

        [SerializeField]
        private float initialSpeed = 0;

        [SerializeField]
        private GameObject indicatorParticles;

        [SerializeField]
        private float m_explosionRatius;

        [SerializeField]
        private FXReceiver explosionParticles;

        [SerializeField]
        private GameObject activationParticles;

        [SerializeField]
        TrembleConfig trembleFall;

        [SerializeField]
        TrembleConfig trembleExplosion;

        [SerializeField]
        private float timeBetweenFire;

        [SerializeField]
        private float fireGrowDuration;

        [SerializeField]
        private float fireStableDuration;

        [SerializeField]
        private FireController fireScript;

        [SerializeField]
        private float timeBetweenJumps;

        [SerializeField]
        private float jumpHeight;

        [SerializeField]
        private FXReceiver jumpReceiver;

        [SerializeField]
        private Collider m_runnerDetectorTrigger;

        [SerializeField]
        private Collider m_skillBreakTrigger;

        [SerializeField]
        private Collider bossSpawnCheckCollider;

        #endregion

        #region Variables

        private Renderer m_renderer;
        private Vector3 finalPos;
        private Vector3 speed = Vector3.zero;
        private bool destroyOnLanding = false;
        private Transform fatherTransformStorage;
        private SimulateChildOf simulateChildOf;
        private float fatherInitialY;

        #endregion

        private void Awake()
        {
            m_renderer = GetComponentInChildren<Renderer>();
            simulateChildOf = GetComponent<SimulateChildOf>();
            Debug.Assert(m_renderer != null);
        }

        private void OnEnable()
        {
            m_runnerDetectorTrigger.enabled = false;
            m_skillBreakTrigger.enabled = false;

            speed.y = initialSpeed;

            SetInitialTiling(m_renderer.material);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            if (fireScript != null)
            {
                fireScript.Stop();
            }
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
            speed.y = initialSpeed;
            destroyOnLanding = false;
        }

        protected override void OnSkillStart()
        {
            RaycastHit info;
            if (Physics.Raycast(transform.position, Vector3.down, out info, rayCheckerLenght, Layers.Stage))
            {
                if (info.collider.CompareTag(Tags.Water))
                {
                    destroyOnLanding = true;
                }
                finalPos = transform.position + Vector3.down * info.distance;
                fatherTransformStorage = info.collider.gameObject.transform;
                fatherInitialY = fatherTransformStorage.position.y;
            }

            FXManager.Instance.InstantiateFromValues(transform.position, indicatorParticles.transform.rotation, indicatorParticles);

            StartCoroutine(FadeInAndFall());
        }

        private IEnumerator FadeInAndFall()
        {
            yield return StartCoroutine(GenerateTrap());
            m_runnerDetectorTrigger.enabled = true;
            m_skillBreakTrigger.enabled = true;
            yield return StartCoroutine(Fall());

            if (phase != SkillBase.Phase.PHASE1)
            {
                StartCoroutine(Fire());
            }
            if (phase == SkillBase.Phase.PHASE3)
            {
                StartCoroutine(Jump());
            }
        }

        private IEnumerator GenerateTrap()
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

        private IEnumerator Fall() // @TODO does this work 100% reliably with falling platforms and moving platforms¿?
        {
            while (true)
            {
                transform.Translate(speed * Time.deltaTime);
                float yVar = (fatherTransformStorage != null ? fatherTransformStorage.position.y - fatherInitialY : 0);
                if (transform.position.y < finalPos.y + yVar)
                {
                    finalPos.y += yVar;
                    transform.position = finalPos;
                    simulateChildOf.Parent = fatherTransformStorage;
                    if (destroyOnLanding)
                    {
                        Explode();
                    }
                    break;
                }
                speed.y += gravity * Time.deltaTime;
                yield return null;
            }
            TrembleManager.Instance.Tremble(trembleFall);
            AudioManager.Instance.PlaySFX(m_trapfallClip);
        }

        IEnumerator Fire()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeBetweenFire);
                fireScript.Play(fireGrowDuration, fireStableDuration);
            }
        }

        IEnumerator Jump()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeBetweenJumps);
                fatherInitialY = transform.parent.position.y;
                finalPos = transform.position;
                jumpReceiver.PlayFx();
                yield return StartCoroutine(Jump(jumpHeight));
            }
        }

        IEnumerator Jump(float height)
        {
            speed.y = HeightToVelocity(jumpHeight);
            while (speed.y > 0)
            {
                transform.Translate(speed * Time.deltaTime);
                speed.y += gravity * Time.deltaTime;
                yield return null;
            }
            yield return StartCoroutine(Fall());
        }

        private void SetInitialTiling(Material mat)
        {
            if (mat.HasProperty("_Noise"))
            {
                float x = Mathf.Sin(Time.time);
                float y = Mathf.Cos(Time.time);
                mat.SetTextureOffset("_Noise", new Vector2(x, y));
            }
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

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_explosionRatius);
        }

        private float HeightToVelocity(float height)
        {
            return Mathf.Sqrt(height * -2.0f * gravity);
        }

        void IBossSkillBreakable.Break()
        {
            Debug.Assert(gameObject.activeSelf);
            if (gameObject.activeSelf)
            {
                Explode();
            }
        }
    }
}