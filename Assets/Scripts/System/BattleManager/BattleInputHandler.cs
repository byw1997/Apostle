using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using TMPro;
#if UNITY_EDITOR
using NUnit.Framework;
#endif

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

    public TextMeshProUGUI skillUI;

    void Awake()
    {
        battleManager = GetComponent<BattleManager>();
        pathfinder = new Pathfinder(TilemapManager.tileMap);
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

        if (battleManager.selectedSkill < 0 || cachedSkillRanges[battleManager.selectedSkill] == null)
        {
            return;
        }

        ShowSkillPreview();


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

            if (tile)
            {
                if (lastMouseOveredTile != tile)
                {
                    lastMouseOveredTile = tile;
                }
                if(battleManager.selectedSkill == -1)
                {
                    return;
                }
                Skill skill = battleManager.currentCharacter.skillSet[battleManager.selectedSkill];
                int level = battleManager.currentCharacter.skillLevel[battleManager.selectedSkill];
                if (IsSkillAvailableForPlayer(tile, skill))
                {
                    skillUI.transform.position = Input.mousePosition;
                    ShowSkillUI();
                    Dictionary<Vector2Int, Pathfinder.Node> skillArea = null;
                    if (skill is AOESkill aoeSkill)
                    {
                        skillArea = pathfinder.CalculateSkillArea(tile, aoeSkill, level);
                        ShowSkillAreaPreview(skillArea);
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        battleManager.currentCharacter.UseSkill(tile, battleManager.selectedSkill, skillArea);
                        EmptyCache();
                        battleManager.Transition(BattleInputMode.Move);
                    }
                }
                else
                {
                    UnShowSkillUI();
                }

            }
        }

    }

    public void ShowSkillUI()//정보 표시 수정 필요
    {
        Skill skill = battleManager.currentCharacter.skillSet[battleManager.selectedSkill];
        skillUI.text = skill.skillName;
        skillUI.gameObject.SetActive(true);
    }

    public void UnShowSkillUI()
    {
        skillUI.gameObject.SetActive(false);
    }

    public void ShowSkillPreview()
    {
        Skill skill = battleManager.currentCharacter.skillSet[battleManager.selectedSkill];
        switch (skill)
        {
            case TargetingSingleSkill:
                foreach(var tile in cachedSkillRanges[battleManager.selectedSkill])
                {
                    if(IsSkillAvailableForPlayer(TilemapManager.tileMap[tile.Key], skill))
                    {
                        tilemapManager.HighlightTile(tile.Key, BattleInputMode.Skill, true);
                    }
                    else
                    {
                        tilemapManager.HighlightTile(tile.Key, BattleInputMode.Skill, false);
                    }
                }
                break;
            //Case for TargetingAreaSkill
            default:
                foreach(var tile in cachedSkillRanges[battleManager.selectedSkill])
                {
                    tilemapManager.HighlightTile(tile.Key, BattleInputMode.Skill, false);
                }
                break;
        }
    }

    public void ShowSkillAreaPreview(Dictionary<Vector2Int, Pathfinder.Node> skillArea)
    {
        foreach (var pos in skillArea)
        {
            tilemapManager.HighlightTile(pos.Key, BattleInputMode.Skill, true);
        }
    }

    public bool IsSkillAvailableForPlayer(Tile tile, Skill skill)
    {
        if(cachedSkillRanges[battleManager.selectedSkill] == null || !cachedSkillRanges[battleManager.selectedSkill].ContainsKey(tile.gridPos))
        {
            return false;
        }
        if(skill.skillTarget == SkillTarget.All)
        {
            return true;
        }
        else if (tile && tile.objectOnTile)
        {
            Character characterOnTile = tile.objectOnTile.GetComponent<Character>();

            if (characterOnTile != null)
            {
                if(skill.skillTarget == SkillTarget.Self && characterOnTile == battleManager.currentCharacter)
                {
                    return true;
                }
                else if(skill.skillTarget == SkillTarget.Ally && (characterOnTile is Player || characterOnTile is Companion))
                {
                    return true;
                }
                else if (skill.skillTarget == SkillTarget.Enemy && characterOnTile is Enemy)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void CalculateSkillRange(Character character, int index)
    {
        if(index == -1)
        {
            return;
        }
        if(cachedSkillRanges[index] != null)
        {
            tilemapManager.HighlightAll(BattleInputMode.Skill, cachedSkillRanges[index]);
            return;
        }
        Dictionary<Vector2Int, Pathfinder.Node> skillRange = null;
        
        skillRange = pathfinder.CalculateSkillRange(character, index);
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
                    EmptyCache();
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
