using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortoiseShell : MonoBehaviour {

    public float sinkDelay = 0.5f;
    public float emergeDelay = 0.5f;
    public float speed = 10.0f;
    public float timeAction = 0.5f;

    private float timer= 0.0f;
    private enum State{ SINKING,EMERGING,NONE };
    private State state = State.NONE;
    private Collider trigger;
    private bool changingStateFlag = false;

    private void Start()
    {
        foreach(Collider c in GetComponents<Collider>())
        {
            if (c.isTrigger)
            {
                trigger = c;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        switch (state)
        {
            case State.EMERGING:
                {
                    if (timer <= timeAction)
                    {
                        transform.Translate(new Vector3(0,0 , speed * Time.deltaTime));
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        trigger.enabled = true;
                        state = State.NONE;
                        timer = 0.0f;
                    }
                }
                break;
            case State.SINKING:
                if (timer <= timeAction)
                {
                    transform.Translate(new Vector3(0, 0 , -speed * Time.deltaTime));
                    timer += Time.deltaTime;
                }
                else
                {
                    if (!changingStateFlag)
                    {
                        StartCoroutine(ChangeStateDelayed(State.EMERGING, emergeDelay));                     
                    }
                }
                break;
            case State.NONE:
                break;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            trigger.enabled = false;
            StartCoroutine(ChangeStateDelayed(State.SINKING,sinkDelay));
        }
    }

    IEnumerator ChangeStateDelayed(State s,float delay)
    {
        changingStateFlag = true;
        yield return new WaitForSeconds(delay);
        changingStateFlag = false;
        timer = 0.0f;
        state = s;
    }
}
