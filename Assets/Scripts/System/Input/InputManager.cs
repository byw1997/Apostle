using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class InputManager : MonoBehaviour
{
    [Header("Input Medium")]
    [SerializeField] private BattleManager battleManager;

    public void HandleInput(GState state)
    {
        switch (state)
        {
            case GState.Default:
                HandleDefaultInput();
                break;
            case GState.Field:
                HandleFieldInput();
                break;
            case GState.Battle:
                HandleBattleInput();
                break;
            case GState.Dialogue:
                HandleDialogueInput();
                break;
            case GState.Menu:
                HandleMenuInput();
                break;
        }
    }

    void HandleDefaultInput()
    {

    }

    void HandleFieldInput()
    {

    }

    void HandleBattleInput()
    {
        battleManager.HandleInput();
    }

    void HandleDialogueInput()
    {

    }

    void HandleMenuInput()
    {

    }
}
