using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    private Text text;

    private bool on = false;

    private float time = 0.0f;

	void Awake()
    {
        text = GetComponent<Text>();
	}

	void Update()
    {
		if(on)
        {
            time -= Time.deltaTime;

            if(time <= 0.0f)
            {
                on = false;
                time = 0.0f;
            }

            text.text = Mathf.Round(time).ToString();
        }
	}

    public void Go(float newTime)
    {
        on = true;
        time = newTime;
    }
}