using UnityEngine;

public abstract class GameState : MonoBehaviour, IState
{
    [SerializeField] protected InputManager inputManager;
    public virtual void EnterState() { }

    public virtual void ExitState() { }

    public virtual void UpdateState() { }
}
