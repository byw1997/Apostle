using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

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
    public List<GameObject> charactersToDeploy = new List<GameObject>();
    private int currentDeployIndex;
    public TilemapManager tilemapManager;

    Tile lastMouseOveredTile = null;

    Pathfinder pathfinder;

    private Dictionary<Vector2Int, Pathfinder.Node> cachedReachableTiles = null;

    void Awake()
    {
        battleManager = GetComponent<BattleManager>();
        pathfinder = new Pathfinder(tilemapManager.tileMap);
    }

    public void HandleInput(BattleInputMode currentMode)
    {
        switch(currentMode)
        {
            case BattleInputMode.Deploy:
                HandleInputDeploy();
                break;
            case BattleInputMode.Idle:
                HandleInputIdle();
                break;
            case BattleInputMode.Skill:
                HandleInputSkill();
                break;
            case BattleInputMode.Move:
                HandleInputMove();
                break;
        }
    }

    public void StartDeployment(List<GameObject> playerCharactersOnBattle)
    {
        charactersToDeploy = playerCharactersOnBattle;
        currentDeployIndex = 0;
        tilemapManager.HighlightAll(BattleInputMode.Deploy);
    }

    public void HandleInputDeploy()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask tileLayer = LayerMask.GetMask("Tile");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
        {
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();
            
            if (tile && tile.deployable)
            {
                if(lastMouseOveredTile != tile)
                {
                    lastMouseOveredTile = tile;
                }
                tile.Preview(charactersToDeploy[currentDeployIndex]);
                if (Input.GetMouseButtonDown(0))
                {
                    tile.Deploy(charactersToDeploy[currentDeployIndex++]);
                }
            }
        }
        if (currentDeployIndex >= charactersToDeploy.Count)
        {
            tilemapManager.UnhighlightAll();
            battleManager.EndDeploy();
        }
    }

    public void HandleInputIdle()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if(battleManager.currentCharacter == null)
        {
            battleManager.TurnForNextCharacter();
        }
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

    public void HandleInputMove()//입력 시 gridpos로 dict에서 검색해서 이동가능한지 파악. Idle로 전환 후 순차이동. 
    {
        if (cachedReachableTiles == null)
        {
            CalculateMovable();
        }

        Assert.IsNotNull(cachedReachableTiles);
        
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask tileLayer = LayerMask.GetMask("Tile");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
            {
                Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                if (tile && cachedReachableTiles.ContainsKey(tile.gridPos))
                {
                    MoveCurrentCharacter(tile, cachedReachableTiles[tile.gridPos]);
                }
            }
        }
    }

    public void CalculateMovable()
    {
        Dictionary<Vector2Int, Pathfinder.Node> reachableTiles = null;
        reachableTiles = pathfinder.CalculateMoveRange(battleManager.currentCharacter, battleManager.currentCharacter.moveType);
        Assert.IsNotNull(reachableTiles);
        cachedReachableTiles = reachableTiles;
        tilemapManager.HighlightAll(BattleInputMode.Move, reachableTiles);
    }

    private void MoveCurrentCharacter(Tile tile, Pathfinder.Node node)
    {
        battleManager.currentCharacter.Move(tile);
        tilemapManager.UnhighlightAll();
        cachedReachableTiles = null;
    }

}
