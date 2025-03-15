using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

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
    }

    public void HandleInputDeploy()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask tileLayer = LayerMask.GetMask("Tile");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
            {
                Tile tile = hit.collider.gameObject.GetComponent<Tile>();

                if (tile)
                {
                    tile.Deploy(charactersToDeploy[currentDeployIndex++]);
                }
            }
            if (currentDeployIndex >= charactersToDeploy.Count)
            {
                Transition(BattleInputMode.Idle);
            }
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
