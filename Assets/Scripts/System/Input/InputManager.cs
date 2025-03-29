using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class InputManager : MonoBehaviour
{
    [Header("Input Medium")]
    [SerializeField] private BattleManager battleManager;

    [Header("Camera Settings")]
    public Camera cam;
    [SerializeField] private float cameraSpeed = 12f;
    public float zoomSpeed = 40f; // ¡‹ º”µµ
    public float minZoom = 10f; // √÷º“ ¡‹ ∞™
    public float maxZoom = 120f; // √÷¥Î ¡‹ ∞™
    public CameraController cameraController;

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

    }
}
