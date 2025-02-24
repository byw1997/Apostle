using UnityEngine;

public interface IInputHandler<T>
{ 
    void HandleInput(T inputMode);
}
