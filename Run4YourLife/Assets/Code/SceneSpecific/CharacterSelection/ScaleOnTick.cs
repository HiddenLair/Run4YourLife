using UnityEngine;
using System.Collections;

public class ScaleOnTick : MonoBehaviour
{
    [SerializeField]
    private float scaleMultiplier;

    [SerializeField]
    private float timeS;

    private Vector3 initialScale;
    private Vector3 targetScale;

    private Coroutine tickCoroutine;

    void OnEnable()
    {
        initialScale = transform.localScale;
        targetScale = scaleMultiplier * initialScale;
    }

    public void Tick()
    {
        if(tickCoroutine != null)
        {
            StopCoroutine(tickCoroutine);
        }

        tickCoroutine = StartCoroutine(Scale());
    }

    private IEnumerator Scale()
    {
        transform.localScale = initialScale;

        float initialTimeS = Time.time;

        float remainingTimeS = timeS;
        float remainingTimeSDiv2 = 0.5f * remainingTimeS;

        while(remainingTimeS >= 0.0f)
        {
            float t = Mathf.PingPong(Time.time - initialTimeS, remainingTimeSDiv2);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t / remainingTimeSDiv2);

            remainingTimeS -= Time.unscaledDeltaTime;

            yield return null;
        }

        transform.localScale = initialScale;
    }
}