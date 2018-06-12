using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Run4YourLife.Player {
    public class EarthPike : SkillBase {

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

        private void OnEnable()
        {
            
        }
    }
}
