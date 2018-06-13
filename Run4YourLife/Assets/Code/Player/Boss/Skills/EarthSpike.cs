using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Run4YourLife.Player {
    public class EarthSpike : SkillBase {

        #region Inspector

        [SerializeField]
        private float width;

        [SerializeField]
        private float delayHit;

        [SerializeField]
        private float timeToGrow;

        [SerializeField]
        private float timeToBreak;

        [SerializeField]
        private GameObject adviseParticles;

        [SerializeField]
        private GameObject earthPikeEffect;

        #endregion

        #region Variables

        private int maximumIterNumToCheck = 5;
        private float maxPercent;
        //Add a little offset to vectors, in order to raycast well
        float offset = 0.1f;

        #endregion

        public override bool Check()
        {
            Vector3 raycastPosition = transform.position;
            Collider[] colliders = Physics.OverlapBox(transform.position,new Vector3(0.1f,0.1f,0.1f),Quaternion.identity,Layers.Stage,QueryTriggerInteraction.Ignore);
            if(colliders.Length != 0)
            {
                //Allways take the left collider
                Collider leftCollider = null;
                float xOffset = Mathf.Infinity;
                foreach(Collider c in colliders)
                {
                    if(c.bounds.center.x < xOffset)
                    {
                        xOffset = c.bounds.center.x;
                        leftCollider = c;
                    }
                }

                //Especial Case
                if (leftCollider.tag == Tags.Wall)
                {
                    SpawnUnder(leftCollider);
                    return true;
                }

                Transform currentIterTransform = leftCollider.transform;
                StageInfo info = currentIterTransform.GetComponent<StageInfo>();
                while(info == null)
                {
                    info = currentIterTransform.parent.GetComponent<StageInfo>();
                    Debug.Assert(currentIterTransform == transform.root);
                }

                Vector3 newPos = transform.position;
                Camera mainCamera = Camera.main;
                float bottomScreen = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, Math.Abs(mainCamera.transform.position.z - transform.position.z))).y;
                if (!info.GetMinValue(out raycastPosition.y) || raycastPosition.y < bottomScreen)
                {
                    newPos.y = info.GetTopValue();
                    float topScreen = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, Math.Abs(mainCamera.transform.position.z - transform.position.z))).y;
                    if (newPos.y > topScreen)
                    {
                        return false;
                    }
                    transform.position = newPos;
                    transform.SetParent(currentIterTransform);
                    return true;
                }
            }

            RaycastHit hitInfo;
            if (Physics.Raycast(raycastPosition, Vector3.down,out hitInfo, Mathf.Infinity, Layers.Stage, QueryTriggerInteraction.Ignore))
            {
                if(hitInfo.collider.tag == Tags.Wall)
                {
                    SpawnUnder(hitInfo.collider);
                    return true;
                }
                transform.position = hitInfo.point;
                transform.SetParent(hitInfo.transform);
                return true;
            }

            return false;
        }

        private void SpawnUnder(Collider collider)
        {
            Vector3 newPos = transform.position;
            newPos.y = collider.bounds.min.y;
            transform.position = newPos;
        }

        public override void StartSkill()
        {
            float radius = width / 2;
            Vector3 centerPos = transform.position;
            centerPos.y += offset;//Added offset to check
            float leftMax = CheckForGround(new Vector3(centerPos.x - radius,centerPos.y,centerPos.z),centerPos,false);
            float rightMax = CheckForGround(centerPos,new Vector3(centerPos.x + radius, centerPos.y, centerPos.z),true);

            float xOffset = leftMax - centerPos.x + rightMax - centerPos.x;
            transform.Translate(new Vector3(xOffset/4,0,0));

            float maxLenght = rightMax - leftMax;
            maxPercent = maxLenght / width;

            StartCoroutine(StartAdvise());
        }

        private float CheckForGround(Vector3 min, Vector3 max, bool takeMax)
        {
            return CheckForGround(min,max,takeMax,1);
        }

        private float CheckForGround(Vector3 min, Vector3 max,bool takeMax, int iterationNumber)
        {
            bool minCheck = takeMax;
            bool maxCheck = !takeMax;
            minCheck |= Physics.Raycast(min, Vector3.down, offset*2, Layers.Stage, QueryTriggerInteraction.Ignore);
            maxCheck |= Physics.Raycast(max, Vector3.down, offset*2, Layers.Stage, QueryTriggerInteraction.Ignore);

            if(minCheck && maxCheck)
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
            else { 
                if (minCheck)
                {
                    Vector3 mid = max;
                    float radius = width / 2;
                    float actualRadiusCheck= radius / (2* iterationNumber);
                    mid.x -= actualRadiusCheck;
                    if (Physics.Raycast(mid, Vector3.down, offset*2, Layers.Stage, QueryTriggerInteraction.Ignore))
                    {
                        return CheckForGround(mid,max,takeMax,++iterationNumber);
                    }
                    return CheckForGround(min,mid,takeMax,++iterationNumber);
                }
                else
                {
                    Vector3 mid = min;
                    float radius = width / 2;
                    float actualRadiusCheck = radius / (2 * iterationNumber);
                    mid.x += actualRadiusCheck;
                    if (Physics.Raycast(mid, Vector3.down, offset, Layers.Stage, QueryTriggerInteraction.Ignore))
                    {
                        return CheckForGround(min, mid, takeMax,++iterationNumber);
                    }
                    return CheckForGround(mid,max,takeMax,++iterationNumber);
                }
            }           
        }

        IEnumerator StartAdvise()
        {
            adviseParticles.transform.localScale = new Vector3(maxPercent * width, maxPercent * width, maxPercent * width);
            adviseParticles.SetActive(true);
            yield return new WaitForSeconds(delayHit);
            adviseParticles.SetActive(false);
            StartCoroutine(StartPikeGrow());
        }

        IEnumerator StartPikeGrow()
        {
            Vector3 currentScale = transform.localScale = Vector3.zero;
            earthPikeEffect.SetActive(true);
            float growBySecond = maxPercent / timeToGrow;
            Vector3 increaseVector = new Vector3(growBySecond,growBySecond,growBySecond);
            while (currentScale.magnitude < maxPercent)
            {
                currentScale += increaseVector * Time.deltaTime;
                transform.localScale = currentScale * width;
                yield return null;
            }
            yield return new WaitForSeconds(timeToBreak);
            earthPikeEffect.SetActive(false);
            Break();
        }
        private void Break()
        {
            //We should make a copy of the pike, but allready broken, and break it here
            gameObject.SetActive(false);
        }
    }
}
