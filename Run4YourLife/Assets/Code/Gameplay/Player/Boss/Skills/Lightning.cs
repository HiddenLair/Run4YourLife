using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.Player {
    public class Lightning : MonoBehaviour {

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

        private void Start()
        {
            StartCoroutine(Flash());
        }

        IEnumerator Flash()
        {
            Transform flashBody = flashEffect.transform;
            Vector3 newSize = Vector3.one;
            newSize.x = newSize.z = width;
            float topScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Mathf.Abs(Camera.main.transform.position.z - flashBody.position.z))).y;
            newSize.y = topScreen - transform.position.y;
            flashBody.localScale = newSize;
            flashBody.localPosition = new Vector3(0, newSize.y);
            flashEffect.SetActive(true);
            yield return new WaitForSeconds(delayHit);
            flashEffect.SetActive(false);
            LightningHit();
        }

        private void LightningHit()
        {
            Vector3 pos = Vector3.zero;
            pos.y = Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, Mathf.Abs(Camera.main.transform.position.z - pos.z))).y;
            lighningEffect.transform.localPosition = pos;
            lighningEffect.SetActive(true);

            RaycastHit[] hits;
            int layerMask = 1 << LayerMask.NameToLayer("Runner");
            hits = Physics.SphereCastAll(lighningEffect.transform.position, width, Vector3.down, pos.y - transform.position.y, layerMask);

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
