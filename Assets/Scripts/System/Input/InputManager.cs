using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public enum MenuState
{
    Options,
    Inventory,
    Skills,
    Quests,
    Map,
    None
}

public class InputManager : MonoBehaviour
{
    [Header("Input Medium")]
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private UIManager uiManager;

    [Header("Camera Settings")]
    public Camera cam;
    [SerializeField] private float cameraSpeed = 12f;
    public float zoomSpeed = 40f; // ¡‹ º”µµ
    public float minZoom = 10f; // √÷º“ ¡‹ ∞™
    public float maxZoom = 120f; // √÷¥Î ¡‹ ∞™
    public CameraController cameraController;

    private MenuState currentMenuState = MenuState.None;

    public void HandleInput(GState state)
    {
        switch (state)
        {
            case GState.Default:
                HandleDefaultInput();
                break;
            case GState.Field:
                HandleMenuInput();
                HandleScreenInput();
                HandleFieldInput();
                break;
            case GState.Battle:
                HandleMenuInput();
                HandleScreenInput();
                HandleBattleInput();
                break;
            case GState.Dialogue:
                HandleDialogueInput();
                break;
        }
    }

    void HandleScreenInput()
    {
        HandleCameraZoom();
        HandleCameraMove();
    }

    private void HandleCameraMove()
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

    private void HandleCameraZoom()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        if (cam.orthographic)
        {
            cam.orthographicSize -= scrollData * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            cam.fieldOfView -= scrollData * zoomSpeed;
            cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentMenuState)
            {
                case MenuState.Options:
                    UnShowOption();
                    break;
                case MenuState.Inventory:
                    UnShowInventory();
                    break;
                case MenuState.Skills:
                    UnShowSkills();
                    break;
                case MenuState.Quests:
                    break;
                case MenuState.Map:
                    break;
                case MenuState.None:
                    ShowOption();
                    break;
            } 
        }
    }

    void ShowOption()
    {
        currentMenuState = MenuState.Options;
        Time.timeScale = 0f;
        uiManager.ShowOptionUI();
    }

    void UnShowOption()
    {
        currentMenuState = MenuState.None;
        Time.timeScale = 1f;
        uiManager.UnShowOptionUI();
    }

    void UnShowInventory()
    {
        currentMenuState = MenuState.None;
        Time.timeScale = 1f;
        uiManager.UnShowInventoryUI();
    }

    void UnShowSkills()
    {
        currentMenuState = MenuState.None;
        Time.timeScale = 1f;
        uiManager.UnShowSkillsUI();
    }
}
