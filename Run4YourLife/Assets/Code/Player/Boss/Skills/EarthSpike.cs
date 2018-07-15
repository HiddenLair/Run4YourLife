using System;
using System.Collections;

using UnityEngine;

using Run4YourLife.CameraUtils;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Player
{
    public class EarthSpike : SkillBase
    {

        #region Inspector

        [SerializeField]
        private Vector3 m_spawnColliderBounds;

        [SerializeField]
        private float delayHit;

        [SerializeField]
        [Tooltip("Time it takes the earth spike to fully grow")]
        private float m_timeToGrow;

        [SerializeField]
        [Tooltip("Time it takes after the earth spike to be fully grown, to be broken")]
        private float m_timeToBreak;

        [SerializeField]
        private GameObject m_warningSpikeParticles;

        [SerializeField]
        private GameObject m_earthSpikeChild;

        [SerializeField]
        private TrembleConfig m_spikeGrowTrembleConfig;

        [SerializeField]
        private GameObject m_breakableWallPrefab;

        #endregion

        /*
        public override bool CanBePlacedAt(ref SkillSpawnData skillSpawnData)
        {
            Vector3 raycastPosition = skillSpawnData.position;
            Collider[] colliders = Physics.OverlapBox(skillSpawnData.position, new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity, Layers.Stage, QueryTriggerInteraction.Ignore);
            if (colliders.Length > 0)
            {
                Collider collider = LeftMostCollider(colliders);

                //Especial Case
                if (collider.CompareTag(Tags.Wall))
                {
                    SpawnUnder(collider, ref skillSpawnData);
                    return true;
                }

                StageInfo info = FindStageInfoForCollider(collider);
                Debug.Assert(info != null);

                Camera mainCamera = CameraManager.Instance.MainCamera;
                float bottomScreen = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(0, 0)).y;

                float minValue;
                if (!info.GetMinValue(out minValue) || minValue < bottomScreen)
                {
                    Vector3 newPos = transform.position;
                    newPos.y = info.GetTopValue();
                    float topScreen = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1, 1)).y;
                    if (newPos.y > topScreen)
                    {
                        return false;
                    }
                    skillSpawnData.position = newPos;
                    skillSpawnData.parent = info.transform;
                    return true;
                }
            }
            


            RaycastHit hitInfo;
            if (Physics.Raycast(raycastPosition, Vector3.down, out hitInfo, Mathf.Infinity, Layers.Stage, QueryTriggerInteraction.Ignore))
            {
                if (hitInfo.collider.CompareTag(Tags.Wall))
                {
                    SpawnUnder(hitInfo.collider, ref skillSpawnData);
                    return true;
                }
                skillSpawnData.position = hitInfo.point;
                skillSpawnData.parent = hitInfo.transform;
                return true;
            }

            return false;
        }
        */


        public override bool CheckAndRepositionSkillSpawn(ref SkillSpawnData skillSpawnData)
        {
            Collider[] stageElements;
            if (!CrosshairInsideStageElements(skillSpawnData.position, out stageElements))
            {
                //Crosshair is in the air
                RaycastHit validStageElement;
                if (FindValidElementToSpawnAbove(skillSpawnData.position, out validStageElement)) // Cast ray below to find spawn position
                {
                    UpdateSkillSpawnDataSpawnLocation(validStageElement.collider, validStageElement.point, ref skillSpawnData);
                    return true;
                }
            }
            else
            {
                //TODO
                Debug.Log("Earth spike inside stage element still not implemented");
            }

            return false;
        }

        private bool CrosshairInsideStageElements(Vector3 position, out Collider[] stageElements)
        {
            stageElements = Physics.OverlapBox(position, new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity, Layers.Stage, QueryTriggerInteraction.Ignore);
            return stageElements.Length > 0;
        }

        private bool FindValidElementToSpawnAbove(Vector3 position, out RaycastHit validElement)
        {
            RaycastHit[] hits = Physics.RaycastAll(position, Vector3.down, Mathf.Infinity, Layers.Stage, QueryTriggerInteraction.Ignore);
            foreach (RaycastHit hit in hits)
            {
                if (!hit.collider.CompareTag(Tags.Wall)) // we can't use walls as the ground for spawning it
                {
                    validElement = hit;
                    return true;
                }
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
            if (colliders.Length > 0)
            {
                bool leftPentrating = colliders[0].bounds.center.x <= position.x;
                if(leftPentrating) // Would penetrate a wall on its left
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


        /*private StageInfo FindStageInfoForCollider(Collider collider)
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

        private static Collider LeftMostCollider(Collider[] colliders)
        {
            Collider leftCollider = null;
            float xOffset = Mathf.Infinity;
            foreach (Collider c in colliders)
            {
                if (c.bounds.center.x < xOffset)
                {
                    xOffset = c.bounds.center.x;
                    leftCollider = c;
                }
            }

            return leftCollider;
        }

        private void SpawnUnder(Collider collider, ref SkillSpawnData skillSpawnData)
        {
            Vector3 newPos = transform.position;
            newPos.y = collider.bounds.min.y;
            skillSpawnData.position = newPos;
            Transform actualParent = collider.transform.parent;
            while (actualParent.tag == Tags.Wall)
            {
                actualParent = actualParent.parent;
            }
            skillSpawnData.parent = actualParent;
        }*/

        protected override void ResetState()
        {
            StopAllCoroutines();
            m_warningSpikeParticles.SetActive(false);
            m_earthSpikeChild.SetActive(false);
        }

        /*protected override void OnSkillStart()
        {
            float radius = width / 2;
            Vector3 centerPos = transform.position;
            centerPos.y += offset;//Added offset to check
            float leftMax = CheckForGround(new Vector3(centerPos.x - radius, centerPos.y, centerPos.z), centerPos, false);
            float rightMax = CheckForGround(centerPos, new Vector3(centerPos.x + radius, centerPos.y, centerPos.z), true);

            float xOffset = leftMax - centerPos.x + rightMax - centerPos.x;
            transform.Translate(new Vector3(xOffset / 4, 0, 0));

            float maxLenght = rightMax - leftMax;
            maxPercent = maxLenght / width;

            StartCoroutine(StartAdvise());
        }

        private float CheckForGround(Vector3 min, Vector3 max, bool takeMax)
        {
            return CheckForGround(min, max, takeMax, 1);
        }

        private float CheckForGround(Vector3 min, Vector3 max, bool takeMax, int iterationNumber)
        {
            bool minCheck = takeMax;
            bool maxCheck = !takeMax;
            minCheck |= Physics.Raycast(min, Vector3.down, offset * 2, Layers.Stage, QueryTriggerInteraction.Ignore);
            maxCheck |= Physics.Raycast(max, Vector3.down, offset * 2, Layers.Stage, QueryTriggerInteraction.Ignore);

            if (minCheck && maxCheck)
            {
                if (takeMax)
                {
                    return max.x;
                }
                else
                {
                    return min.x;
                }
            }
            else if (iterationNumber > maximumIterNumToCheck)//Exit if to many iterations
            {
                if (minCheck)
                {
                    return min.x;
                }
                else
                {
                    return max.x;
                }
            }
            else
            {
                if (minCheck)
                {
                    Vector3 mid = max;
                    float radius = width / 2;
                    float actualRadiusCheck = radius / (2 * iterationNumber);
                    mid.x -= actualRadiusCheck;
                    if (Physics.Raycast(mid, Vector3.down, offset * 2, Layers.Stage, QueryTriggerInteraction.Ignore))
                    {
                        return CheckForGround(mid, max, takeMax, ++iterationNumber);
                    }
                    return CheckForGround(min, mid, takeMax, ++iterationNumber);
                }
                else
                {
                    Vector3 mid = min;
                    float radius = width / 2;
                    float actualRadiusCheck = radius / (2 * iterationNumber);
                    mid.x += actualRadiusCheck;
                    if (Physics.Raycast(mid, Vector3.down, offset, Layers.Stage, QueryTriggerInteraction.Ignore))
                    {
                        return CheckForGround(min, mid, takeMax, ++iterationNumber);
                    }
                    return CheckForGround(mid, max, takeMax, ++iterationNumber);
                }
            }
        }
        
        IEnumerator StartAdvise()
        {
            AudioManager.Instance.PlaySFX(m_skillTriggerClip);
            adviseParticles.transform.localScale = new Vector3(maxPercent * width, maxPercent * width, maxPercent * width);
            adviseParticles.SetActive(true);
            yield return new WaitForSeconds(delayHit);
            adviseParticles.SetActive(false);
            StartCoroutine(StartPikeGrow());
        }

        IEnumerator StartPikeGrow()
        {
            TrembleManager.Instance.Tremble(trembleConfig);
            Vector3 currentScale = transform.localScale = Vector3.zero;
            earthPikeEffect.SetActive(true);
            float growBySecond = maxPercent / timeToGrow;
            Vector3 increaseVector = new Vector3(growBySecond, growBySecond, growBySecond);
            while (CheckForSpace() && currentScale.magnitude < maxPercent)
            {
                currentScale += increaseVector * Time.deltaTime;
                transform.localScale = currentScale * width;
                transform.Translate(0, 0, 0);
                yield return null;
            }
            yield return new WaitForSeconds(timeToBreak);
            earthPikeEffect.SetActive(false);
            Break();
        }

        private bool CheckForSpace()
        {
            bool ret = false;
            RaycastHit info;
            ret |= Physics.Raycast(stageCollider.bounds.center, Vector3.right, out info, stageCollider.bounds.extents.x, Layers.Stage, QueryTriggerInteraction.Ignore);
            ret |= Physics.Raycast(stageCollider.bounds.center, Vector3.left, out info, stageCollider.bounds.extents.x, Layers.Stage, QueryTriggerInteraction.Ignore);
            ret |= Physics.Raycast(stageCollider.bounds.center, Vector3.up, out info, stageCollider.bounds.extents.y, Layers.Stage, QueryTriggerInteraction.Ignore);

            return !ret;
        }

        private void Break()
        {
            if (phase != SkillBase.Phase.PHASE1)
            {
                GameObject instance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(wallGameObject, transform.position, Quaternion.identity, true);
                Vector3 currentScale = new Vector3(maxPercent, maxPercent, maxPercent);
                instance.transform.localScale = currentScale * width;
                instance.transform.SetParent(transform.parent);
            }

            //We should make a copy of the pike, but allready broken, and break it here

            gameObject.SetActive(false);
        }
        */

        protected override void OnSkillStart()
        {
            StartCoroutine(SkillBehaviuour());
        }

        private IEnumerator SkillBehaviuour()
        {
            AudioManager.Instance.PlaySFX(m_skillTriggerClip);

            // Display Ground Particles for a short amount of time
            m_warningSpikeParticles.SetActive(true);
            yield return new WaitForSeconds(delayHit);
            m_warningSpikeParticles.SetActive(false);

            TrembleManager.Instance.Tremble(m_spikeGrowTrembleConfig);

            // Earth spike grows
            m_earthSpikeChild.SetActive(true);
            float endTime = Time.time + m_timeToGrow;
            while (Time.time < endTime)
            {
                float percentage = 1f - ((endTime - Time.time) / m_timeToGrow);
                transform.localScale = Vector3.one * percentage;
                yield return null;
            }

            yield return new WaitForSeconds(m_timeToBreak);
            m_earthSpikeChild.SetActive(false);

            SpawnBreakableWall();

            gameObject.SetActive(false);
        }

        private void SpawnBreakableWall()
        {
            if (phase != SkillBase.Phase.PHASE1)
            {
                GameObject instance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(m_breakableWallPrefab, transform.position, Quaternion.identity, true);
                instance.transform.SetParent(transform.parent);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position + new Vector3(0, m_spawnColliderBounds.y / 2f), m_spawnColliderBounds);
        }
    }
}
