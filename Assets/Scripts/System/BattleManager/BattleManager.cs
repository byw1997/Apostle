using UnityEngine;
using System.Collections.Generic;
public enum BattleInputMode
{
    Idle,
    Skill,
    Move
}
public class BattleManager : MonoBehaviour
{
    public BattleInputMode currentMode;

    private BattleInputHandler battleInputHandler;

    private List<Character> charactersOnBattle = new List<Character>();

    private Skill selectedSkill = null;
    private Character currentCharacter = null;

    private void Awake()
    {
        battleInputHandler = GetComponent<BattleInputHandler>();
    }

    private void Start()
    {
        currentMode = BattleInputMode.Idle;
    }

    public void HandleInput()
    {
        battleInputHandler.HandleInput(currentMode);
    }
    
}
