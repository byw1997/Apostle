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

    private KeyCode[] keyCodes = {
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
    };

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

    public void NextCharacterDeployment()
    {
        currentDeployIndex++;
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
            
            if (tile && tile.deployable && tile.objectOnTile == null)
            {
                if(lastMouseOveredTile != tile)
                {
                    lastMouseOveredTile = tile;
                }
                tile.Preview(charactersToDeploy[currentDeployIndex]);
                if (Input.GetMouseButtonDown(0))
                {
                    tile.Deploy(charactersToDeploy[currentDeployIndex]);
                    NextCharacterDeployment();
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
            battleManager.EndTurn();
        }
        if (Input.GetMouseButtonDown(0))
        {
            // ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask tileLayer = LayerMask.GetMask("Tile");
            // Raycast�� �浹�� ��ü�� ã��
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
            {
                Tile tile = hit.collider.gameObject.GetComponent<Tile>();
                // Ŭ���� ��ġ�� Plane�� ������ �� Plane�� ��ġ ���
                if (tile)
                {
                    tile.OutputTileInfo();
                }
            }
        }
    }
    
    public void HandleInputSkill()
    {
        SelectSkillByShortcut();
    }

    public void HandleInputMove()//�Է� �� gridpos�� dict���� �˻��ؼ� �̵��������� �ľ�. Idle�� ��ȯ �� �����̵�. 
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (cachedReachableTiles == null)
        {
            CalculateMovable();
        }

        Assert.IsNotNull(cachedReachableTiles);

        SelectSkillByShortcut();

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
        battleManager.currentCharacter.Move(tile, node);
        tilemapManager.UnhighlightAll();
        cachedReachableTiles = null;
    }

    private void SelectSkillByShortcut()
    {
        for (int i = 1; i <= 6; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                battleManager.SelectSkill(i - 1);
            }
        }
    }
}
