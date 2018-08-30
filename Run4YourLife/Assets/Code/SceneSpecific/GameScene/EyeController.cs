using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class EyeController : MonoBehaviour
{
    [SerializeField]
    private float minTimeVisibleS = 0.5f;

    [SerializeField]
    private float maxTimeVisibleS = 1.0f;

    [SerializeField]
    private float minTimeBetweenVisibleS = 1.0f;

    [SerializeField]
    private float maxTimeBetweenVisibleS = 10.0f;

    [SerializeField]
    private float minScaleMultiplier = 0.75f;

    [SerializeField]
    private float maxScaleMultiplier = 1.25f;

    private Coroutine coroutine;
    private Vector3 initialScale;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        initialScale = transform.localScale;
    }

    void OnEnable()
    {
        coroutine = StartCoroutine(Controller());
    }

    void OnDisable()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator Controller()
    {
        while(true)
        {
            transform.localScale = initialScale * Random.Range(minScaleMultiplier, maxScaleMultiplier);

            yield return new WaitForSeconds(Random.Range(minTimeBetweenVisibleS, maxTimeBetweenVisibleS));

            meshRenderer.enabled = true;

            yield return new WaitForSeconds(Random.Range(minTimeVisibleS, maxTimeVisibleS));

            if(Random.Range(0.0f, 1.0f) >= 0.8f)
            {
                meshRenderer.enabled = false;

                yield return new WaitForSeconds(0.15f * Random.Range(minTimeVisibleS, maxTimeVisibleS));

                meshRenderer.enabled = true;
            }

            yield return new WaitForSeconds(Random.Range(minTimeVisibleS, maxTimeVisibleS));

            meshRenderer.enabled = false;
        }
    }
}