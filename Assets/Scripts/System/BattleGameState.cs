using UnityEngine;

public class BattleGameState : GameState
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private CharacterManager characterManager;

    public override void EnterState()
    {
        //LoadData();
        EnterBattle();
    }

    public override void ExitState()
    {
        uiManager.UnShowBattleUI();
    }

    public override void UpdateState()
    {
        inputManager.HandleInput(GState.Battle);
    }

    private void LoadData()
    {
        if (!battleManager.IsLoaded)
        {
            battleManager.LoadAllEnemiesFromAddressables();
        }
    }

    public void EnterBattle()
    {
        battleManager.playerCharactersOnBattle.Add(characterManager.playerCharacter);
        battleManager.playerCharactersOnBattle.AddRange(characterManager.activeCompanionCharacters);
        uiManager.ShowBattleUI();
        StartCoroutine(battleManager.InitializeBattle());
    }
}
