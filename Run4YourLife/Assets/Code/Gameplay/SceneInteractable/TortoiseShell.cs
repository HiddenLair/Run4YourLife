using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortoiseShell : MonoBehaviour {

    public float sinkDelay = 0.5f;
    public float emergeDelay = 0.5f;
    public float speed = 10.0f;
    public float timeAction = 0.5f;

    List<Transform> players = new List<Transform>();
    private float timer= 0.0f;
    private enum State{ SINKING,EMERGING,NONE };
    private State state = State.NONE;
    private bool triggerLogicActive = true;
    private bool changingStateFlag = false;

    // Update is called once per frame
    void Update () {
        switch (state)
        {
            case State.EMERGING:
                {
                    if (timer <= timeAction)
                    {
                        MovePlayers(speed);
                        transform.Translate(new Vector3(0,0 , speed * Time.deltaTime));
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        triggerLogicActive = true;
                        state = State.NONE;
                        timer = 0.0f;
                    }
                }
                break;
            case State.SINKING:
                if (timer <= timeAction)
                {
                    MovePlayers(-speed);
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

    private void MovePlayers(float speedValue)
    {
        foreach (Transform transform in players)
        {
            transform.Translate(new Vector3(0, speedValue * Time.deltaTime, 0), Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Player))
        {
            players.Add(other.transform);
            if (triggerLogicActive)
            {
                triggerLogicActive = false;
                StartCoroutine(ChangeStateDelayed(State.SINKING, sinkDelay));
            }
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            players.Remove(other.transform);
        }
    }
}
