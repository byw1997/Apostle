using UnityEngine;
using System.Collections.Generic;
public enum BattleInputMode
{
    Deploy,
    Idle,
    Skill,
    Move
}
public class BattleManager : MonoBehaviour
{
    public BattleInputMode currentMode;

    private BattleInputHandler battleInputHandler;

    public List<GameObject> playerCharactersOnBattle = new List<GameObject>();
    public List<Character> charactersOnBattle = new List<Character>();

    private Skill selectedSkill = null;
    private Character currentCharacter = null;

    public static BattleManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        battleInputHandler = GetComponent<BattleInputHandler>();
    }

    public void HandleInput()
    {
        battleInputHandler.HandleInput(currentMode);
    }

    public void Transition(BattleInputMode nextMode)
    {
        currentMode = nextMode;
    }

    public void InitializeBattle()
    {
        currentMode = BattleInputMode.Deploy;
        battleInputHandler.StartDeployment(playerCharactersOnBattle);
    }
    
    private void DeployEnemy()
    {

    }

    public void EndDeploy()
    {
        foreach (GameObject character in playerCharactersOnBattle)
        {
            charactersOnBattle.Add(character.GetComponent<Character>());
        }
        DeployEnemy();
        Transition(BattleInputMode.Idle);
    }
}
