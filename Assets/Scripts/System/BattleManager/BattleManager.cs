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
    private List<Character> charactersOnBattle = new List<Character>();

    private Skill selectedSkill = null;
    private Character currentCharacter = null;

    private void Awake()
    {
        battleInputHandler = GetComponent<BattleInputHandler>();
    }

    public void HandleInput()
    {
        battleInputHandler.HandleInput(currentMode);
    }

    public void InitializeBattle()
    {
        currentMode = BattleInputMode.Deploy;
        battleInputHandler.StartDeployment(playerCharactersOnBattle);
    }
    
}
