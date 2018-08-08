using System;
using System.Collections;

using UnityEngine;

using Run4YourLife.CameraUtils;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.Interactables;
using System.Linq;

namespace Run4YourLife.Player.Boss.Skills.EarthSpike
{
    public class EarthSpikeController : SkillBase, IBossSkillBreakable
    {
        [SerializeField]
        private Vector3 m_spawnColliderBounds;

        [SerializeField]
        [Tooltip("On screen duration of the earth spike warning particles")]
        private float m_warningParticlesDuration;

        [SerializeField]
        [Tooltip("Time it takes the earth spike to fully grow")]
        private float m_earthSpikeGrowthDuration;

        [SerializeField]
        [Tooltip("After being fully grown, delay to be broken")]
        private float m_breakEarthSpikeDelay;

        [SerializeField]
        private FXReceiver m_spawnParticles;

        [SerializeField]
        private GameObject m_earthSpikeGraphics;

        [SerializeField]
        private Collider m_bossSkillBreakTrigger;

        [SerializeField]
        private TrembleConfig m_spikeGrowTrembleConfig;

        [SerializeField]
        private GameObject m_breakableWallPrefab;

        private Vector3 m_initialLocalScale;

        private SimulateChildOf m_simulateChildOf;


        private void Awake()
        {
            m_initialLocalScale = transform.localScale;
            m_simulateChildOf = GetComponent<SimulateChildOf>();
        }

        public override bool CheckAndRepositionSkillSpawn(ref SkillSpawnData skillSpawnData)
        {
            Collider[] stageElements;
            if (!CrosshairInsideStageElements(skillSpawnData.position, out stageElements))
            {
                //Crosshair is in the air
                RaycastHit validStageElement;
                if (FindValidStageElementToSpawnAbove(skillSpawnData.position, out validStageElement)) // Cast ray below to find spawn position
                {
                    UpdateSkillSpawnDataSpawnLocation(validStageElement.collider, validStageElement.point, ref skillSpawnData);
                    return true;
                }
            }
            else
            {
                Collider stageCollider = null;
                Vector3 position = skillSpawnData.position;

                if (SelectStageElementColliderToSpawnAbove(stageElements, ref stageCollider, ref position))
                {
                    UpdateSkillSpawnDataSpawnLocation(stageCollider, position, ref skillSpawnData);
                }
                return true;
            }

            return false;
        }

        private bool SelectStageElementColliderToSpawnAbove(Collider[] colliders, ref Collider collider, ref Vector3 position)
        {
            StageInfo info = FindStageInfoForCollider(colliders[0]);
            Debug.Assert(info != null);

            Camera mainCamera = CameraManager.Instance.MainCamera;
            float topScreenHeightPosition = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1, 1)).y;
            float topStageElementHeightPosition = info.GetTopValue();

            if (topStageElementHeightPosition < topScreenHeightPosition)
            {
                collider = colliders[0];
                position = new Vector3()
                {
                    x = position.x,
                    y = topStageElementHeightPosition,
                    z = position.z
                };
                return true;
            }
            return false;
        }

        private bool CrosshairInsideStageElements(Vector3 position, out Collider[] stageElements)
        {
            Collider[] colliders = Physics.OverlapBox(position, new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity, Layers.Stage, QueryTriggerInteraction.Ignore);
            stageElements = colliders.Where(x => !x.CompareTag(Tags.Wall)).ToArray();

            return stageElements.Length > 0;
        }

        private bool FindValidStageElementToSpawnAbove(Vector3 position, out RaycastHit validElement)
        {
            RaycastHit[] hits = Physics.RaycastAll(position, Vector3.down, Mathf.Infinity, Layers.Stage, QueryTriggerInteraction.Ignore);
            hits = hits.Where(x => !x.collider.CompareTag(Tags.Wall)).ToArray();

            if (hits.Length > 0)
            {
                int min = 0;
                for (int i = 1; i < hits.Length; i++)
                {
                    if (hits[i].distance < hits[min].distance)
                    {
                        min = i;
                    }
                }
                validElement = hits[min];
                return true;
            }

            validElement = new RaycastHit();
            return false;
        }

        /// Collider for the object
        /// Position to place the object. position.y should be equals to collider.bounds.max.y
        /// SkillSpawnData to update
        private void UpdateSkillSpawnDataSpawnLocation(Collider collider, Vector3 position, ref SkillSpawnData skillSpawnData)
        {
            Vector3 halfBounds = m_spawnColliderBounds / 2f;
            Collider[] colliders = Physics.OverlapBox(position + new Vector3(0, halfBounds.y, 0), halfBounds * 0.9f, Quaternion.identity, Layers.Stage, QueryTriggerInteraction.Ignore);
            colliders = colliders.Where(x => !x.CompareTag(Tags.Wall)).ToArray();

            if (colliders.Length > 0)
            {
                bool leftPentrating = colliders[0].bounds.center.x <= position.x;
                if (leftPentrating) // Would penetrate a wall on its left
                {
                    position.x = colliders[0].bounds.max.x + halfBounds.x;
                }
                else // Would penetrate a wall on its right
                {
                    position.x = colliders[0].bounds.min.x - halfBounds.x;
                }
            }
            else
            {
                if (!Physics.Raycast(position + new Vector3(-halfBounds.x, 0.05f, 0), Vector3.down, 0.1f, Layers.Stage, QueryTriggerInteraction.Ignore)) // Not Left Grounded
                {
                    position.x = collider.bounds.min.x + halfBounds.x;
                }
                else if (!Physics.Raycast(position + new Vector3(halfBounds.x, 0.05f, 0), Vector3.down, 0.1f, Layers.Stage, QueryTriggerInteraction.Ignore)) // Not Right Grounded
                {
                    position.x = collider.bounds.max.x - halfBounds.x;
                }
            }

            skillSpawnData.position = position;
            skillSpawnData.parent = collider.transform;
        }

        private StageInfo FindStageInfoForCollider(Collider collider)
        {
            Transform t = collider.transform;
            StageInfo stageInfo = t.GetComponent<StageInfo>();
            while (stageInfo == null && t != transform.root)
            {
                t = t.parent;
                stageInfo = t.GetComponent<StageInfo>();
            }
            return stageInfo;
        }

        protected override void ResetState()
        {
            StopAllCoroutines();
            m_earthSpikeGraphics.SetActive(false);
            m_bossSkillBreakTrigger.enabled = false;
            transform.localScale = Vector3.zero;
        }

        protected override void OnSkillStart()
        {
            StartCoroutine(SkillBehaviuour());
        }

        private IEnumerator SkillBehaviuour()
        {
            AudioManager.Instance.PlaySFX(m_skillTriggerClip);

            // Display Ground Particles for a short amount of time
            m_spawnParticles.PlayFx(false);
            yield return new WaitForSeconds(m_warningParticlesDuration);

            TrembleManager.Instance.Tremble(m_spikeGrowTrembleConfig);

            // Earth spike grows
            m_bossSkillBreakTrigger.enabled = true;
            m_earthSpikeGraphics.SetActive(true);
            float endTime = Time.time + m_earthSpikeGrowthDuration;
            while (Time.time < endTime)
            {
                float percentage = 1f - ((endTime - Time.time) / m_earthSpikeGrowthDuration);
                transform.localScale = m_initialLocalScale * percentage;
                yield return null;
            }

            yield return new WaitForSeconds(m_breakEarthSpikeDelay);
            m_earthSpikeGraphics.SetActive(false);

            SpawnBreakableWall();

            gameObject.SetActive(false);
        }

        private void SpawnBreakableWall()
        {
            if (phase != SkillBase.Phase.PHASE1)
            {
                GameObject instance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_breakableWallPrefab, transform.position, Quaternion.identity, true);
                instance.GetComponent<SimulateChildOf>().Parent = m_simulateChildOf.Parent;
            }
        }

        void IBossSkillBreakable.Break()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(0, m_spawnColliderBounds.y / 2f), m_spawnColliderBounds);
        }
    }
}
