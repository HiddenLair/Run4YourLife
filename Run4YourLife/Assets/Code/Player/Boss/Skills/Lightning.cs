using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.GameManagement;

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

        #endregion

        private void OnEnable()
        {
            Vector3 position = transform.position;
            position.y = CameraManager.Instance.MainCamera.ScreenToWorldPoint(new Vector3(0, 0, Mathf.Abs(CameraManager.Instance.MainCamera.transform.position.z - transform.position.z))).y;
            transform.position = position;
            StartCoroutine(Flash());
        }

        IEnumerator Flash()
        {       
            Transform flashBody = flashEffect.transform;
            Vector3 newSize = Vector3.one;
            newSize.x = newSize.z = width;
            float topScreen = CameraManager.Instance.MainCamera.ScreenToWorldPoint(new Vector3(0, CameraManager.Instance.MainCamera.pixelHeight, Mathf.Abs(CameraManager.Instance.MainCamera.transform.position.z - flashBody.position.z))).y;
            newSize.y = (topScreen - transform.position.y)/2;
            flashBody.localScale = newSize;
            flashBody.localPosition = new Vector3(0, newSize.y);
            flashEffect.SetActive(true);
            yield return new WaitForSeconds(delayHit);
            flashEffect.SetActive(false);
            LightningHit();
        }

        private void LightningHit()
        {
            Camera mainCamera = CameraManager.Instance.MainCamera;
            Vector3 pos = Vector3.zero;
            pos.y = mainCamera.ScreenToWorldPoint(new Vector3(0, mainCamera.pixelHeight, Mathf.Abs(mainCamera.transform.position.z - pos.z))).y;
            lighningEffect.transform.localPosition = pos;
            lighningEffect.SetActive(true);

            RaycastHit[] hits;
            hits = Physics.SphereCastAll(lighningEffect.transform.position, width, Vector3.down, pos.y - transform.position.y,Layers.Runner);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == Tags.Runner)
                {
                    ExecuteEvents.Execute<ICharacterEvents>(hit.collider.gameObject, null, (x, y) => x.Kill());
                }
            }
        }
    }
}
