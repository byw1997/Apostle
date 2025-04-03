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
    private Dictionary<Vector2Int, Pathfinder.Node>[] cachedSkillRanges = new Dictionary<Vector2Int, Pathfinder.Node>[6]
{
    null, null, null, null, null, null
};

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
        if (battleManager.currentCharacter.status != CharacterStatus.Idle)
        {
            return;
        }

        CalculateSkillRange(battleManager.currentCharacter, battleManager.selectedSkill);
        SelectSkillByShortcut();

        
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        foreach(var range in cachedSkillRanges[battleManager.selectedSkill])
        {
            Debug.Log(range.Key);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask tileLayer = LayerMask.GetMask("Tile");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
        {
            Tile tile = hit.collider.gameObject.GetComponent<Tile>();

            if (tile && tile.deployable && tile.objectOnTile == null)
            {
                if (lastMouseOveredTile != tile)
                {
                    lastMouseOveredTile = tile;
                }
                if (Input.GetMouseButtonDown(0))
                {

                }
            }
        }

    }

    public void CalculateSkillRange(Character character, int index)
    {
        if(cachedSkillRanges[index] != null)
        {
            tilemapManager.HighlightAll(BattleInputMode.Skill, cachedSkillRanges[index]);
            return;
        }
        Dictionary<Vector2Int, Pathfinder.Node> skillRange = null;
        
        skillRange = pathfinder.CalculateSkillRange(character, index);
        Assert.IsNotNull(skillRange);
        cachedSkillRanges[index] = skillRange;
        tilemapManager.HighlightAll(BattleInputMode.Skill, skillRange);
    }

    public void HandleInputMove()//최적 경로 보강 필요(교전 거리 등)
    {
        if(battleManager.currentCharacter.status != CharacterStatus.Idle)
        {
            return;
        }

        CalculateMovable();
        SelectSkillByShortcut();

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        Assert.IsNotNull(cachedReachableTiles);



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
        if (cachedReachableTiles != null)
        {
            tilemapManager.HighlightAll(BattleInputMode.Move, cachedReachableTiles);
            return;
        }
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
        EmptyCache();
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

    public void EndTurn()
    {
        EmptyCache();
    }

    public void EmptyCache()
    {
        cachedReachableTiles = null;
        for (int i = 0; i < cachedSkillRanges.Length; i++)
        {
            cachedSkillRanges[i] = null;
        }
    }
}
