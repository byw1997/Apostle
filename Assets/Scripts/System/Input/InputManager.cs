using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class InputManager : MonoBehaviour
{
    [Header("Input Medium")]
    [SerializeField] private BattleManager battleManager;

    [Header("Camera Settings")]
    [SerializeField] private float cameraSpeed = 12f;

    public void HandleInput(GState state)
    {
        switch (state)
        {
            case GState.Default:
                HandleDefaultInput();
                break;
            case GState.Field:
                HandleScreenInput();
                HandleFieldInput();
                break;
            case GState.Battle:
                HandleScreenInput();
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

    void HandleScreenInput()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        Camera.main.transform.position += direction * cameraSpeed * Time.deltaTime;
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
