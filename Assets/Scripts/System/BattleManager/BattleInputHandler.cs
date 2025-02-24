using UnityEngine;

public class BattleInputHandler : MonoBehaviour, IInputHandler<BattleInputMode>
{
    enum TileType
    {
        Empty,
        Controllable,
        Neutral,
        Enemy
    }
    private BattleManager battleManager;
    void Awake()
    {
        battleManager = GetComponent<BattleManager>();
    }

    public void HandleInput(BattleInputMode currentMode)
    {
        switch(currentMode)
        {
            case BattleInputMode.Idle:
                HandleInputIdle();
                break;
            case BattleInputMode.Skill:
                break;
            case BattleInputMode.Move:
                break;
        }
    }
    public void HandleInputIdle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 화면 좌표를 월드 좌표로 변환
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask tileLayer = LayerMask.GetMask("Tile");
            // Raycast로 충돌한 객체를 찾기
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
            {
                Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                // 클릭한 위치가 Plane에 닿으면 그 Plane의 위치 출력
                if (tile)
                {
                    tile.OutputTileInfo();
                }
            }
        }
    }
    
    public void HandleInputSkill()
    {

    }

    public void HandleInputMove()
    {

    }



}
