using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;
using Run4YourLife.CameraUtils;
using Run4YourLife.Utils;

namespace Run4YourLife.Player.Boss.Skills.Lightning
{
    public class LightningController : SkillBase
    {
        #region Inspector

        [SerializeField]
        private AudioClip thunderHitClip;

        [SerializeField]
        private AudioClip cloudEmergingClip;

        [SerializeField]
        private float width;

        [SerializeField]
        private float delayHit;

        [SerializeField]
        private GameObject cloudEffect;

        [SerializeField]
        private GameObject lighningEffect;

        [SerializeField]
        private TrembleConfig trembleConfig;

        [SerializeField]
        private GameObject electricFieldGameObject;

        [SerializeField]
        private float delayBetweenLightnings;

        [SerializeField]
        private float delayBetweenLightningsProgresion;

        [SerializeField]
        private float newLightningsDelayHit;

        [SerializeField]
        private float newLightningsDelayHitProgresion;

        [SerializeField]
        private float newLightningsDistance;

        [SerializeField]
        private float newLightningsDistanceProgresion;

        [SerializeField]
        private GameObject lightningGameObject;

        [SerializeField]
        private int maxNumberOfLightningsPerSide;

        #endregion

        #region Private Variables

        private WaitForSeconds lightningDelay;
        private bool rightLightningSideFinished = false;
        private bool leftLightningSideFinished = false;
        private bool mainLightningFinished = false;
        private float maxIterationModificable = 0.0f;

        #endregion

        private void Awake()
        {
            lightningDelay = new WaitForSeconds(delayHit);
        }

        protected override void ResetState()
        {
            rightLightningSideFinished = false;
            leftLightningSideFinished = false;
            mainLightningFinished = false;
            maxIterationModificable = maxNumberOfLightningsPerSide;
        }

        protected override void OnSkillStart()
        {
            Vector3 position = transform.position;
            position.y = CameraConverter.ViewportToGamePlaneWorldPosition(CameraManager.Instance.MainCamera, new Vector2(0, 0)).y;
            transform.position = position;
            StartCoroutine(Cloud());
            if (phase == SkillBase.Phase.PHASE3)
            {
                StartCoroutine(StartNewLeftLightning(1, transform.position));
                StartCoroutine(StartNewRightLightning(1, transform.position));
            }
        }

        IEnumerator Cloud()
        {
            AudioManager.Instance.PlaySFX(cloudEmergingClip);
            Transform flashBody = cloudEffect.transform;
            Vector3 newSize = Vector3.one;

            float topScreen = CameraConverter.ViewportToGamePlaneWorldPosition(CameraManager.Instance.MainCamera, new Vector2(0, 1)).y;
            float yPos = (topScreen - transform.position.y) - cloudEffect.GetComponent<Renderer>().bounds.extents.y;// / 2;
            flashBody.localPosition = new Vector3(0, yPos, -1);
            cloudEffect.SetActive(true);
            StartCoroutine(HoldCloudTop());
            yield return lightningDelay;            
            LightningHit();
        }

        IEnumerator HoldCloudTop()
        {
            while (true)
            {
                float topScreen = CameraConverter.ViewportToGamePlaneWorldPosition(CameraManager.Instance.MainCamera, new Vector2(0, 1)).y;
                float yPos = (topScreen - transform.position.y) - cloudEffect.GetComponent<Renderer>().bounds.extents.y;// / 2;
                cloudEffect.transform.localPosition = new Vector3(0, yPos, -1);
                yield return null;
            }
        }

        private void LightningHit()
        {
            AudioManager.Instance.PlaySFX(thunderHitClip);
            TrembleManager.Instance.Tremble(trembleConfig);
            Camera mainCamera = CameraManager.Instance.MainCamera;
            Vector3 pos = Vector3.zero;

            float topScreen = CameraConverter.ViewportToGamePlaneWorldPosition(CameraManager.Instance.MainCamera, new Vector2(0, 1)).y;
            pos.y = topScreen - transform.position.y;
            lighningEffect.transform.localPosition = pos;

            LayerMask finalMask = Layers.Runner | Layers.Stage;

            RaycastHit[] hits;
            hits = Physics.SphereCastAll(lighningEffect.transform.position, width / 2, Vector3.down, pos.y - transform.position.y, finalMask, QueryTriggerInteraction.Ignore);

            List<RaycastHit> nonRunnersHits = new List<RaycastHit>();
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag(Tags.Runner))
                {
                    ExecuteEvents.Execute<IRunnerEvents>(hit.collider.gameObject, null, (x, y) => x.Kill());
                }
                else
                {
                    nonRunnersHits.Add(hit);
                }
            }
            if (phase != SkillBase.Phase.PHASE1)
            {
                SetElectricFields(nonRunnersHits);
            }
            lighningEffect.SetActive(true);
            lighningEffect.GetComponent<ParticleScaler>().SetXScale(width);
            ParticleSystem[] lightning = lighningEffect.GetComponentsInChildren<ParticleSystem>();
            StartCoroutine(WaitForParticleSystems(lightning, lighningEffect));
        }

        private void SetElectricFields(List<RaycastHit> hits)
        {

            const float delta = 0.15f;

            List<RaycastHit> spawnFieldHits = new List<RaycastHit>();
            hits = hits.OrderBy(a => a.distance).ToList();
            for (int i = 0; i < hits.Count; ++i)
            {
                if (hits[i].collider.CompareTag(Tags.Runner))
                {
                    continue;
                }
                else
                {
                    spawnFieldHits.Add(hits[i]);
                }

                while (i + 1 < hits.Count)
                {
                    Collider c1 = hits[i].collider;
                    Collider c2 = hits[i + 1].collider;

                    if (c1.bounds.min.y < c2.bounds.max.y + delta)
                    {
                        ++i;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            foreach (RaycastHit hit in spawnFieldHits)
            {
                DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(electricFieldGameObject, hit.point, Quaternion.identity, true);
            }
        }

        IEnumerator WaitForParticleSystems(ParticleSystem[] particles, GameObject particleSystem)
        {
            bool finish = false;
            while (!finish)
            {
                finish = true;
                for (int i = 0; i < particles.Length; ++i)
                {
                    if (particles[i].IsAlive(false))
                    {
                        finish = false;
                        break;
                    }
                }
                yield return null;
            }
            particleSystem.SetActive(false);
            if (phase != Phase.PHASE3 || (rightLightningSideFinished && leftLightningSideFinished))
            {
                gameObject.SetActive(false);
            }
            mainLightningFinished = true;
        }

        public void SetDelayHit(float value)
        {
            delayHit = value;
        }

        public float GetDelayHit()
        {
            return delayHit;
        }

        IEnumerator StartNewLeftLightning(int iterationNumber, Vector3 position)
        {
            yield return new WaitForSeconds(delayBetweenLightnings * Mathf.Pow(delayBetweenLightningsProgresion, iterationNumber));

            position.x -= newLightningsDistance * Mathf.Pow(newLightningsDistanceProgresion, iterationNumber);           
            float leftScreen = CameraConverter.ViewportToGamePlaneWorldPosition(CameraManager.Instance.MainCamera, new Vector2(0, 0)).x;

            if (position.x > leftScreen && iterationNumber <= maxIterationModificable)
            {
                GameObject instance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(lightningGameObject, position, Quaternion.identity, true);
                instance.GetComponent<LightningController>().SetDelayHit(newLightningsDelayHit * Mathf.Pow(newLightningsDelayHitProgresion, iterationNumber));
                instance.GetComponent<SkillBase>().StartSkill();

                StartCoroutine(StartNewLeftLightning(++iterationNumber, position));
            }
            else
            {
                if (rightLightningSideFinished && mainLightningFinished)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    leftLightningSideFinished = true;
                    maxIterationModificable += maxIterationModificable - (iterationNumber - 1);//As we start with iter 1, we have to substract it
                }
            }
        }

        IEnumerator StartNewRightLightning(int iterationNumber, Vector3 position)
        {
            yield return new WaitForSeconds(delayBetweenLightnings * Mathf.Pow(delayBetweenLightningsProgresion, iterationNumber));

            position.x += newLightningsDistance * Mathf.Pow(newLightningsDistanceProgresion, iterationNumber);
            float rightScreen = CameraConverter.ViewportToGamePlaneWorldPosition(CameraManager.Instance.MainCamera, new Vector2(1, 1)).x;

            if (position.x < rightScreen && iterationNumber <= maxIterationModificable)
            {
                GameObject instance = DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(lightningGameObject, position, Quaternion.identity, true);
                instance.GetComponent<LightningController>().SetDelayHit(newLightningsDelayHit * Mathf.Pow(newLightningsDelayHitProgresion, iterationNumber));
                instance.GetComponent<SkillBase>().StartSkill();

                StartCoroutine(StartNewRightLightning(++iterationNumber, position));
            }
            else
            {
                if (leftLightningSideFinished && mainLightningFinished)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    rightLightningSideFinished = true;
                    maxIterationModificable += maxIterationModificable - (iterationNumber-1);//As we start with iter 1, we have to substract it
                }
            }
        }
    }
}
