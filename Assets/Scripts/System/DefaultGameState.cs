using UnityEngine;

public class DefaultGameState : GameState
{
    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        Debug.Log("Current State = Default");
    }
}
