using UnityEngine;
using System.Collections.Generic;
public enum BattleInputMode
{
    Deploy,
    Idle,
    Move,
    Skill
}
public class BattleManager : MonoBehaviour
{
    public BattleInputMode currentMode;

    private BattleInputHandler battleInputHandler;

    public List<GameObject> playerCharactersOnBattle = new List<GameObject>();
    public List<Character> charactersOnBattle = new List<Character>();

    private int currentCharacterIndex = 0;
    private Skill selectedSkill = null;
    public Character currentCharacter = null;

    public static BattleManager Instance { get; private set; }

    public TilemapManager tilemapManager;

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
        tilemapManager.UnhighlightAll();
        switch (nextMode)
        {
            case BattleInputMode.Deploy:
                break;
            case BattleInputMode.Idle:
                break;
            case BattleInputMode.Move:
                battleInputHandler.CalculateMovable();
                break;
            case BattleInputMode.Skill:
                break;
        }
        currentMode = nextMode;
    }

    public void InitializeBattle()
    {
        currentCharacterIndex = 0;
        currentMode = BattleInputMode.Deploy;
        battleInputHandler.StartDeployment(playerCharactersOnBattle);
    }
    
    private void DeployEnemy()
    {
        //Enemies will be added to the charactersOnBattle list
    }

    public void EndDeploy()
    {
        foreach (GameObject character in playerCharactersOnBattle)
        {
            charactersOnBattle.Add(character.GetComponent<Character>());
        }
        DeployEnemy();
        charactersOnBattle.Sort((a, b) => b.dex.CompareTo(a.dex));
        Transition(BattleInputMode.Idle);
    }

    public void TurnForNextCharacter()
    {
        switch(currentMode)
        {
            case BattleInputMode.Move:
                EndTurn();
                break;
            case BattleInputMode.Skill:
                EndTurn();
                break;
        }
        
    }

    public void EndTurn()
    {
        currentCharacter = charactersOnBattle[currentCharacterIndex];
        currentCharacter.InitializeTurn();
        currentCharacterIndex++;
        currentCharacterIndex %= charactersOnBattle.Count;
        if (currentCharacter.GetComponent<Player>() != null || currentCharacter.GetComponent<Companion>() != null)
        {
            Transition(BattleInputMode.Move);
        }
    }

    public void SelectSkill(int i)
    {
        if (selectedSkill == currentCharacter.skillSet[i])
        {
            selectedSkill = null;
            Transition(BattleInputMode.Move);
        }
        else
        {
            selectedSkill = currentCharacter.skillSet[i];
            if (selectedSkill)
            {
                foreach(CustomRange range in selectedSkill.customRanges)
                {
                    Debug.Log(range.tilePositions.Count);
                    foreach (Vector2Int pos in range.tilePositions)
                    {
                        Debug.Log(pos.x);
                    }
                }
                Debug.Log(selectedSkill.customRanges.Length);
                Transition(BattleInputMode.Skill);
            }
        }
    }

    public void UnSelectSkill()
    {
        selectedSkill = null;
        Transition(BattleInputMode.Move);
    }

}
