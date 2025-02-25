using UnityEngine;

public class BattleGameState : GameState
{
    
    public override void EnterState()
    {

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        inputManager.HandleInput(GState.Battle);
    }
}
