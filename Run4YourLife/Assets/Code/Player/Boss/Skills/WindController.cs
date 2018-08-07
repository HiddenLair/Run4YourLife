using System;
using System.Collections;

using UnityEngine;

using Run4YourLife.GameManagement;
using Run4YourLife.CameraUtils;
using Run4YourLife.Player.Runner;

namespace Run4YourLife.Player.Boss.Skills.Wind
{
    public class WindController : SkillBase
    {
        #region Inspector

        [SerializeField]
        [Range(0, 200)]
        private float windMaxForceRelative;

        [SerializeField]
        [Range(0, 100)]
        private float windMinForceRelative;

        [SerializeField]
        [Range(0, 100)]
        private float screenMinWind;

        [SerializeField]
        private float windDuration;

        [SerializeField]
        private float windFillScreenTime;

        [SerializeField]
        private GameObject windParticles;

        [SerializeField]
        private GameObject flyingItem;

        [SerializeField]
        private float timeBetweenFlyingItems;

        [SerializeField]
        private GameObject tornado;

        [SerializeField]
        private float timeBetweenTornados;

        #endregion

        #region Variables

        private float actualFillPercent = 0.0f;

        private float flyingItemTimer = 0.0f;
        private float tornadoTimer = 0.0f;

        #endregion

        protected override void ResetState()
        {
            actualFillPercent = 0.0f;
            tornadoTimer = timeBetweenTornados;
            flyingItemTimer = timeBetweenFlyingItems;
        }

        protected override void OnSkillStart()
        {
            Camera mainCamera = CameraManager.Instance.MainCamera;

            Vector3 bottomLeft = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(0, 0));
            Vector3 topRight = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1, 1));

            Vector3 newPos = new Vector3(topRight.x, mainCamera.transform.position.y, transform.position.z);
            transform.position = newPos;
            Vector3 newScale = new Vector3(0, topRight.y - bottomLeft.y, 2);//z 2 is an arbitrary number, you can change if you need
            transform.localScale = newScale;

            windParticles.SetActive(true);

            StartCoroutine(FillScreen());
        }

        IEnumerator FillScreen()
        {
            Camera mainCamera = CameraManager.Instance.MainCamera;
            float increasePerSec = 100 / windFillScreenTime;
            Vector3 actualScale = transform.localScale;
            Vector3 actualPos = transform.position;
            while (actualFillPercent < 100)
            {
                Vector3 bottomLeft = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(0, 0));
                Vector3 topRight = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1, 1));
                actualFillPercent += increasePerSec * Time.deltaTime;
                actualScale.x = (topRight.x - bottomLeft.x) * actualFillPercent / 100;
                actualScale.y = topRight.y - bottomLeft.y;
                actualPos.x = topRight.x;
                actualPos.y = mainCamera.transform.position.y;
                transform.localScale = actualScale;
                transform.position = actualPos;
                yield return null;
            }
            StartCoroutine(CustomUpdate());
        }

        IEnumerator CustomUpdate()
        {
            Camera mainCamera = CameraManager.Instance.MainCamera;
            float timer = Time.time + windDuration;
            Vector3 actualScale = transform.localScale;
            Vector3 actualPos = transform.position;
            while (timer >= Time.time)
            {
                Vector3 bottomLeft = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(0, 0));
                Vector3 topRight = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1, 1));

                actualScale.x = topRight.x - bottomLeft.x;
                actualScale.y = topRight.y - bottomLeft.y;
                actualPos.x = topRight.x;
                actualPos.y = mainCamera.transform.position.y;

                transform.localScale = actualScale;
                transform.position = actualPos;

                if (phase != SkillBase.Phase.PHASE1)
                {
                    if (flyingItemTimer >= timeBetweenFlyingItems)
                    {
                        Vector3 flyingItemPos = new Vector3(topRight.x, UnityEngine.Random.Range(bottomLeft.y, topRight.y), transform.position.z);
                        DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(flyingItem, flyingItemPos, Quaternion.identity, true);
                        flyingItemTimer = 0.0f;
                    }
                    flyingItemTimer += Time.deltaTime;
                }
                if (phase == SkillBase.Phase.PHASE3)
                {
                    if (tornadoTimer >= timeBetweenTornados)
                    {
                        Vector3 tornadoPos = GetTornadoSpawn();
                        DynamicObjectsManager.Instance.GameObjectPool.GetAndPosition(tornado, tornadoPos, Quaternion.identity, true);
                        tornadoTimer = 0.0f;
                    }
                    tornadoTimer += Time.deltaTime;
                }

                yield return null;
            }
            StartCoroutine(EndWind());
        }

        private Vector3 GetTornadoSpawn()
        {
            //Bounds colliderBounds = GetComponentInChildren<Collider>().bounds;
            //Vector3 pos;
            //pos.z = transform.position.z;
            //while (true)
            //{
            //    pos.x = UnityEngine.Random.Range(colliderBounds.min.x, colliderBounds.max.x);
            //    pos.y = UnityEngine.Random.Range(colliderBounds.min.y, colliderBounds.max.y);

            //    Collider[] colliders = Physics.OverlapBox(pos,Vector3.one,Quaternion.identity,Layers.Stage,QueryTriggerInteraction.Ignore);
            //    if(colliders.Length == 0)
            //    {
            //        return pos;
            //    }
            //}
            Bounds colliderBounds = GetComponentInChildren<Collider>().bounds;
            Vector3 pos;
            pos.z = transform.position.z;

            pos.x = UnityEngine.Random.Range(colliderBounds.min.x, colliderBounds.max.x);
            pos.y = UnityEngine.Random.Range(colliderBounds.min.y, colliderBounds.max.y);

            return pos;
        }

        IEnumerator EndWind()
        {
            Camera mainCamera = CameraManager.Instance.MainCamera;
            Vector3 actualPos = transform.position;
            Vector3 actualScale = transform.localScale;
            float decreasePerSec = 100 / windFillScreenTime;

            while (actualFillPercent > 0)
            {
                Vector3 bottomLeft = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(0, 0));
                Vector3 topRight = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1, 1));

                actualFillPercent -= decreasePerSec * Time.deltaTime;

                actualScale.y = topRight.y - bottomLeft.y;
                actualPos.x = bottomLeft.x + ((topRight.x - bottomLeft.x) * actualFillPercent / 100);
                actualPos.y = mainCamera.transform.position.y;

                transform.position = actualPos;
                transform.localScale = actualScale;

                yield return null;
            }
            windParticles.SetActive(false);
            gameObject.SetActive(false);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                Camera mainCamera = CameraManager.Instance.MainCamera;
                Vector3 bottomLeft = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(0, 0));
                Vector3 topRight = CameraConverter.ViewportToGamePlaneWorldPosition(mainCamera, new Vector2(1, 1));

                float hitOffset = (other.transform.position.x - bottomLeft.x) - ((topRight.x - bottomLeft.x) * screenMinWind / 100);
                float screenOffset = (topRight.x - bottomLeft.x) - ((topRight.x - bottomLeft.x) * screenMinWind / 100);

                float percentaje = hitOffset / screenOffset;

                if (percentaje < 0)
                {
                    other.GetComponent<RunnerController>().WindForceRelative = windMinForceRelative;
                }
                else
                {
                    float increasePerPercentaje = windMaxForceRelative - windMinForceRelative / 100;
                    other.GetComponent<RunnerController>().WindForceRelative = (increasePerPercentaje * percentaje) + windMinForceRelative;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Runner))
            {
                other.GetComponent<RunnerController>().WindForceRelative = 0.0f;
            }
        }
    }
}