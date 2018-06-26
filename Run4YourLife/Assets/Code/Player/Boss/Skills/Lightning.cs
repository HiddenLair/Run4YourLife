using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;
using Run4YourLife.GameManagement;
using Run4YourLife.GameManagement.AudioManagement;

namespace Run4YourLife.Player {
    public class Lightning : SkillBase
    {
        #region Inspector

        [SerializeField]
        private float width;

        [SerializeField]
        private float delayHit;

        [SerializeField]
        private GameObject flashEffect;

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
        private int maxNumberOfLightnings;

        #endregion

        #region Private Variables

        private WaitForSeconds lightningDelay;
        private bool oneLightningSideFinished = false;

        #endregion

        private void Awake()
        {
            lightningDelay = new WaitForSeconds(delayHit);
        }

        protected override void Reset()
        {
            oneLightningSideFinished = false;
        }

        protected override void StartSkillImplementation()
        {
            Vector3 position = transform.position;
            position.y = CameraManager.Instance.MainCamera.ScreenToWorldPoint(new Vector3(0, 0, Mathf.Abs(CameraManager.Instance.MainCamera.transform.position.z - transform.position.z))).y;
            transform.position = position;
            StartCoroutine(Flash());
            if (phase == SkillBase.Phase.PHASE3)
            {
                StartCoroutine(StartNewLeftLightning(1,transform.position));
                StartCoroutine(StartNewRightLightning(1,transform.position));
            }
        }

        IEnumerator Flash()
        {
            Transform flashBody = flashEffect.transform;
            Vector3 newSize = Vector3.one;
            newSize.x = newSize.z = width;
            float topScreen = CameraManager.Instance.MainCamera.ScreenToWorldPoint(new Vector3(0, CameraManager.Instance.MainCamera.pixelHeight, Mathf.Abs(CameraManager.Instance.MainCamera.transform.position.z - flashBody.position.z))).y;
            newSize.y = (topScreen - transform.position.y);// / 2;
            //flashBody.localScale = newSize;
            flashBody.localPosition = new Vector3(0, newSize.y - 0.5f, -1);
            flashEffect.SetActive(true);
            yield return lightningDelay;
            //flashEffect.SetActive(false);
            LightningHit();
        }

        private void LightningHit()
        {
            AudioManager.Instance.PlaySFX(m_skillTriggerClip);
            TrembleManager.Instance.Tremble(trembleConfig);
            Camera mainCamera = CameraManager.Instance.MainCamera;
            Vector3 pos = Vector3.zero;
            pos.y = mainCamera.ScreenToWorldPoint(new Vector3(0, mainCamera.pixelHeight, Mathf.Abs(mainCamera.transform.position.z - pos.z))).y;
            lighningEffect.transform.localPosition = pos;

            LayerMask finalMask = Layers.Runner | Layers.Stage;

            RaycastHit[] hits;
            hits = Physics.SphereCastAll(lighningEffect.transform.position, width/2, Vector3.down, pos.y - transform.position.y,finalMask,QueryTriggerInteraction.Ignore);

            List<RaycastHit> nonRunnersHits = new List<RaycastHit>();
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag(Tags.Runner))
                {
                    ExecuteEvents.Execute<ICharacterEvents>(hit.collider.gameObject, null, (x, y) => x.Kill());
                }
                else
                {
                    nonRunnersHits.Add(hit);
                }
            }
            if(phase != SkillBase.Phase.PHASE1)
            {
                SetElectricFields(nonRunnersHits);
            }

            lighningEffect.SetActive(true);
            ParticleSystem[] lightning = lighningEffect.GetComponentsInChildren<ParticleSystem>();
            StartCoroutine(WaitForParticleSystems(lightning,lighningEffect));
        }

        private void SetElectricFields(List<RaycastHit> hits)
        {

            const float delta = 0.15f;

            List<RaycastHit> spawnFieldHits = new List<RaycastHit>();
            hits = hits.OrderBy(a => a.distance).ToList();
            for(int i = 0;i<hits.Count; ++i)
            {
                if(hits[i].collider.CompareTag(Tags.Runner))
                {
                    continue;
                }
                else
                {
                    spawnFieldHits.Add(hits[i]);
                }

                while (i+1 < hits.Count)
                {
                    Collider c1 = hits[i].collider;
                    Collider c2 = hits[i + 1].collider;

                    if(c1.bounds.min.y < c2.bounds.max.y + delta)
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
                BossPoolManager.Instance.InstantiateBossElement(electricFieldGameObject, hit.point);
            }
        }

        IEnumerator WaitForParticleSystems(ParticleSystem[] particles,GameObject particleSystem)
        {
            bool finish = false;
            while(!finish){
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
            if (phase != SkillBase.Phase.PHASE3)
            {
                gameObject.SetActive(false);
            }
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

            position.x -= newLightningsDistance * Mathf.Pow( newLightningsDistanceProgresion , iterationNumber);
            GameObject instance = BossPoolManager.Instance.InstantiateBossElement(lightningGameObject, position);
            instance.GetComponent<Lightning>().SetDelayHit(newLightningsDelayHit * Mathf.Pow(newLightningsDelayHitProgresion,iterationNumber));
            instance.GetComponent<SkillBase>().StartSkill();

            Camera mainCamera = Camera.main;
            float leftScreen = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, Math.Abs(mainCamera.transform.position.z - transform.position.z))).x;

            if (position.x > leftScreen && iterationNumber < maxNumberOfLightnings)
            {
                StartCoroutine(StartNewLeftLightning(++iterationNumber, position));
            }
            else
            {
                if (oneLightningSideFinished)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    oneLightningSideFinished = true;
                }
            }
        }

        IEnumerator StartNewRightLightning(int iterationNumber,Vector3 position)
        {
            yield return new WaitForSeconds(delayBetweenLightnings * Mathf.Pow(delayBetweenLightningsProgresion,iterationNumber));

            position.x += newLightningsDistance * Mathf.Pow(newLightningsDistanceProgresion, iterationNumber);
            GameObject instance = BossPoolManager.Instance.InstantiateBossElement(lightningGameObject, position);
            instance.GetComponent<Lightning>().SetDelayHit(newLightningsDelayHit * Mathf.Pow(newLightningsDelayHitProgresion, iterationNumber));
            instance.GetComponent<SkillBase>().StartSkill();

            Camera mainCamera = Camera.main;
            float rightScreen = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, Math.Abs(mainCamera.transform.position.z - transform.position.z))).x;

            if (position.x < rightScreen && iterationNumber < maxNumberOfLightnings) {
                StartCoroutine(StartNewRightLightning(++iterationNumber,position));
            }
            else
            {
                if (oneLightningSideFinished)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    oneLightningSideFinished = true;
                }
            }
        }
    }
}
