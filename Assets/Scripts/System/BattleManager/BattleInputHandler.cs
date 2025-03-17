using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

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
    void Awake()
    {
        battleManager = GetComponent<BattleManager>();
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
                break;
            case BattleInputMode.Move:
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
        if(EventSystem.current.IsPointerOverGameObject())
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
            Transition(BattleInputMode.Idle);
        }
    }

    public void HandleInputIdle()
    {
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

    }

    public void HandleInputMove()
    {

    }

    public void Transition(BattleInputMode nextMode)
    {
        battleManager.currentMode = nextMode;
    }

}
