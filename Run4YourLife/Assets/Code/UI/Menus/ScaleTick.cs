using UnityEngine;

public class ScaleTick : MonoBehaviour
{
    [SerializeField]
    private float scaleMultiplier = 1.5f;

    [SerializeField]
    private float timeS = 0.1f;

    private Vector3 initialScale;
    private float timeDiv2S;

    private bool on = false;
    private float currentTimeS = 0.0f;
    private float scaleMultiplierTarget = 1.0f;

	void Awake()
    {
        initialScale = transform.localScale;
        timeDiv2S = 0.5f * timeS;
	}

    void Update()
    {
        if(on)
        {
            currentTimeS += Time.deltaTime;

            if(currentTimeS >= timeDiv2S)
            {
                scaleMultiplierTarget = 1.0f;

                if(currentTimeS >= timeS)
                {
                    on = false;
                }
            }

            transform.localScale = Vector3.Lerp(transform.localScale, scaleMultiplierTarget * initialScale, Time.deltaTime / timeDiv2S);
        }
    }

    public void Tick()
    {
        on = true;
        currentTimeS = 0.0f;
        scaleMultiplierTarget = scaleMultiplier;
    }
}