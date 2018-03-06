using UnityEngine;
using System.Collections.Generic;

public class GameStatesManager : MonoBehaviour
{
    private int current = -1;

    private List<GameState> states = new List<GameState>();

    void Start()
    {
        // Test

        Add(new TestGameState());

        GoNext();
        GoNext();
    }

    public void GoNext()
    {
        if(states.Count > 0)
        {
            Debug.Assert(current == -1 || (current >= 0 && current < states.Count));

            if(current != -1)
            {
                Debug.Assert(states[current] != null);

                states[current].End();
            }

            current = (current + 1) % states.Count;

            Debug.Assert(states[current] != null);

            states[current].Start();
        }
    }

    public void Add(GameState state)
    {
        states.Add(state);
    }

    public void Remove(GameState state)
    {
        states.Remove(state);
    }
}