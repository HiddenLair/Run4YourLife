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

    private Material material;
    private Coroutine coroutine;
    private Vector3 initialScale;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;

        initialScale = transform.localScale;
    }

    void OnEnable()
    {
        Color color = material.color; color.a = 0.0f;
        material.color = color;

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
        float fadeTime = 0.2f;

        while(true)
        {
            transform.localScale = initialScale * Random.Range(minScaleMultiplier, maxScaleMultiplier);

            // 0 .. 0
            yield return new WaitForSeconds(Random.Range(minTimeBetweenVisibleS, maxTimeBetweenVisibleS));

            // 0 -> 1
            yield return StartCoroutine(Fade(true, fadeTime));

            // 1 .. 1
            yield return new WaitForSeconds(0.5f * Random.Range(minTimeVisibleS, maxTimeVisibleS));

            if(Random.Range(0.0f, 1.0f) >= 0.9f)
            {
                // 1 -> 0
                yield return StartCoroutine(Fade(false, 0.25f * fadeTime));

                // 0 .. 0
                yield return new WaitForSeconds(0.05f);

                // 0 -> 1
                yield return StartCoroutine(Fade(true, 0.25f * fadeTime));
            }

            // 1 .. 1
            yield return new WaitForSeconds(0.5f * Random.Range(minTimeVisibleS, maxTimeVisibleS));

            // 1 -> 0
            yield return StartCoroutine(Fade(false, fadeTime));
        }
    }

    private IEnumerator Fade(bool alpha0To1, float timeS)
    {
        float remainingTimeS = timeS;
        Color color = material.color;

        color.a = alpha0To1 ? 0.0f : 1.0f;
        material.color = color;

        while(remainingTimeS >= 0.0f)
        {
            color.a = Mathf.Clamp01(remainingTimeS / timeS);

            if(alpha0To1)
            {
                color.a = 1.0f - color.a;
            }

            material.color = color;
            remainingTimeS -= Time.deltaTime;

            yield return null;
        }

        color.a = alpha0To1 ? 1.0f : 0.0f;
        material.color = color;
    }
}