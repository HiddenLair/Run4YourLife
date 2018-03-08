using UnityEngine;

public class TestGameState : GameState
{
    override public void Start()
    {
        Debug.Log("Start");
    }

    override public void End()
    {
        Debug.Log("End");
    }
}