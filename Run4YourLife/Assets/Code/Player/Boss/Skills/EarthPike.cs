using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;

namespace Run4YourLife.Player {

    [CustomEditor(typeof(EarthPike))]
    [CanEditMultipleObjects]
    public class EarthPikeEditor : BaseSkillEditor
    {
        SerializedProperty earhtPikeGameObject;
        SerializedProperty width;

        private void OnEnable()
        {
            base.Init();
            Init();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(earhtPikeGameObject);
            EditorGUILayout.PropertyField(width);

            SkillBase.Phase actualPhase = (SkillBase.Phase)phase.intValue;
            if (actualPhase == SkillBase.Phase.PHASE2 || actualPhase == SkillBase.Phase.PHASE3)
            {
            }
            if (actualPhase == SkillBase.Phase.PHASE3)
            {
            }
            serializedObject.ApplyModifiedProperties();
        }

        new public void Init()
        {
            earhtPikeGameObject = serializedObject.FindProperty("earhtPikeGameObject");
            width = serializedObject.FindProperty("width");
        }
    }

    public class EarthPike : SkillBase {

        #region Inspector

        [SerializeField]
        private GameObject earhtPikeGameObject;

        [SerializeField]
        private float width;

        #endregion

        #region Variables

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
            float radius = width / 2;
            Vector3 centerPos = transform.position;
            centerPos.y += offset;//Added offset to check
            float leftMax = CheckForGround(new Vector3(centerPos.x - radius,centerPos.y,centerPos.z),centerPos,radius,true);
            float rightMax = CheckForGround(centerPos,new Vector3(centerPos.x + radius, centerPos.y, centerPos.z), radius,false);

            float xOffset = centerPos.x - leftMax + centerPos.x - rightMax;
            transform.Translate(new Vector3(xOffset/2,0,0));

            float maxLenght = rightMax - leftMax;
            maxPercent = maxLenght / radius;

            StartCoroutine(StartAdvise());
        }

        private float CheckForGround(Vector3 min, Vector3 max, float actualRadiusCheck,bool takeMax)
        {
            bool minCheck = Physics.Raycast(min, Vector3.down, offset, Layers.Stage, QueryTriggerInteraction.Ignore);
            bool maxCheck = Physics.Raycast(max, Vector3.down, offset, Layers.Stage, QueryTriggerInteraction.Ignore);

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
            else { 
                if (minCheck)
                {
                    Vector3 mid = max;
                    actualRadiusCheck /= 2;
                    mid.x -= actualRadiusCheck;
                    if (Physics.Raycast(mid, Vector3.down, offset, Layers.Stage, QueryTriggerInteraction.Ignore))
                    {
                        return CheckForGround(mid,max,actualRadiusCheck,takeMax);
                    }
                    return CheckForGround(min,mid,actualRadiusCheck,takeMax);
                }
                else
                {
                    Vector3 mid = min;
                    actualRadiusCheck /= 2;
                    mid.x -= actualRadiusCheck;
                    if (Physics.Raycast(mid, Vector3.down, offset, Layers.Stage, QueryTriggerInteraction.Ignore))
                    {
                        return CheckForGround(min, mid, actualRadiusCheck, takeMax);
                    }
                    return CheckForGround(mid,max,actualRadiusCheck,takeMax);
                }
            }           
        }

        IEnumerator StartAdvise()
        {
            yield return null;
        }

        IEnumerator StartPikeGrow()
        {
            yield return null;
        }
    }
}
