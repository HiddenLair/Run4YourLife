using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpController : MonoBehaviour {

    #region Inspector
    [SerializeField]
    private SkinnedMeshRenderer rightEye;

    [SerializeField]
    private SkinnedMeshRenderer leftEye;

    [SerializeField]
    private SkinnedMeshRenderer head;

    [SerializeField]
    private SkinnedMeshRenderer helmet;

    [SerializeField]
    private float bumpTime = 0.5f;

    #endregion

    public void Bump()
    {
        StartCoroutine(BumpInTime());
    }

    IEnumerator BumpInTime()
    {      
        float growTime = bumpTime/2.0f;
        while (growTime >= 0)
        {
            float weight = 100* (1-(growTime / (bumpTime/2.0f)));
            leftEye.SetBlendShapeWeight(0, weight);
            rightEye.SetBlendShapeWeight(0, weight);
            head.SetBlendShapeWeight(0, weight);
            helmet.SetBlendShapeWeight(0, weight);
            growTime -= Time.deltaTime;
            yield return null;
        }

        float decreaseTime =bumpTime/2.0f;
        while (decreaseTime >= 0)
        {
            float weight = 100 * (decreaseTime / (bumpTime/2.0f));
            leftEye.SetBlendShapeWeight(0, weight);
            rightEye.SetBlendShapeWeight(0, weight);
            head.SetBlendShapeWeight(0, weight);
            helmet.SetBlendShapeWeight(0, weight);
            decreaseTime -= Time.deltaTime;
            yield return null;
        }
    }

}
