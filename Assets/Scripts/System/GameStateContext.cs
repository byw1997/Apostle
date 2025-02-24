using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public enum GState
{
    Default,
    Field,
    Battle,
    Dialogue,
    Menu
}

public class GameStateContext
{
    public IState currentState { get; private set; }
    private readonly GameManager _controller;
    public GameStateContext(GameManager controller)
    {
        _controller = controller;
    }

    public void Transition(IState state)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = state;
        currentState.EnterState();
    }
}
