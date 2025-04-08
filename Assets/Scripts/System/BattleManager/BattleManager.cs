using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
public enum BattleInputMode
{
    Loading,
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
    public int selectedSkill = -1;
    public Character currentCharacter = null;

    public static BattleManager Instance { get; private set; }

    public TilemapManager tilemapManager;
    public UIManager uiManager;

    private Dictionary<int, GameObject> enemyDict = new Dictionary<int, GameObject>();
    public bool IsLoaded { get; private set; } = false;
    public EnemyGroupData enemyGroupData;

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
        LoadAllEnemiesFromAddressables();
    }

    public void LoadAllEnemiesFromAddressables()
    {
        Addressables.LoadAssetsAsync<GameObject>("Enemy", null).Completed += OnEnemiesLoaded;
    }

    private void OnEnemiesLoaded(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var prefab in handle.Result)
            {
                Enemy enemy = prefab.GetComponent<Enemy>();
                if (enemy != null && !enemyDict.ContainsKey(enemy.enemyID))
                {
                    enemyDict.Add(enemy.enemyID, prefab);
                    Debug.Log($"Loaded Enemy ID {enemy.enemyID} from Addressables.");
                }
                else
                {
                    Debug.LogWarning($"Duplicate or invalid Enemy prefab: {prefab.name}");
                }
            }

            IsLoaded = true;
        }
        else
        {
            Debug.LogError("Failed to load Enemy prefabs from Addressables.");
        }
    }
    public GameObject GetEnemyPrefabByID(int id)
    {
        return enemyDict.TryGetValue(id, out var prefab) ? prefab : null;
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
                battleInputHandler.UnShowSkillPreview();
                break;
            case BattleInputMode.Idle:
                battleInputHandler.UnShowSkillPreview();
                break;
            case BattleInputMode.Move:
                battleInputHandler.UnShowSkillPreview();
                selectedSkill = -1;
                battleInputHandler.CalculateMovable();
                break;
            case BattleInputMode.Skill:
                battleInputHandler.CalculateSkillRange(currentCharacter, selectedSkill);
                break;
        }
        currentMode = nextMode;
    }

    public IEnumerator InitializeBattle()
    {
        currentMode = BattleInputMode.Loading;
        while (IsLoaded == false)
        {
            yield return null;
        }
        Debug.Log("Loading Ended");
        currentCharacterIndex = 0;
        currentMode = BattleInputMode.Deploy;
        foreach(GameObject character in playerCharactersOnBattle)
        {
            character.GetComponent<Character>().InitializeBattle();
        }
        
        DeployEnemy();
        battleInputHandler.StartDeployment(playerCharactersOnBattle);
    }
    
    public void DeployEnemy()
    {
        
        //Enemies will be added to the charactersOnBattle list
        for (int i = 0; i < enemyGroupData.enemyIdList.Count; i++)
        {
            GameObject enemyPrefab = GetEnemyPrefabByID(enemyGroupData.enemyIdList[i]);
            if (enemyPrefab != null)
            {
                Tile tile = tilemapManager.tileMap[enemyGroupData.enemyPosList[i]];
                GameObject enemyInstance = Instantiate(enemyPrefab, new Vector3(0,0,0), Quaternion.identity);
                tile.Deploy(enemyInstance);
                enemyInstance.GetComponent<Enemy>().InitializeBattle();
                charactersOnBattle.Add(enemyInstance.GetComponent<Character>());
            }
            else
            {
                Debug.LogError($"Enemy prefab with ID {enemyGroupData.enemyIdList[i]} not found.");
            }
        }
    }

    public void EndDeploy()
    {
        foreach (GameObject character in playerCharactersOnBattle)
        {
            charactersOnBattle.Add(character.GetComponent<Character>());
        }

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
        if (currentCharacter)
        {
            currentCharacter.DisconnectUI();
        }
        battleInputHandler.EndTurn();
        currentCharacter = charactersOnBattle[currentCharacterIndex];
        while(currentCharacter.status == CharacterStatus.Dead)
        {
            currentCharacterIndex++;
            currentCharacterIndex %= charactersOnBattle.Count;
            currentCharacter = charactersOnBattle[currentCharacterIndex];
        }
        currentCharacter.InitializeTurn();
        uiManager.UpdateUI(currentCharacter);
        currentCharacterIndex++;
        currentCharacterIndex %= charactersOnBattle.Count;
        if (currentCharacter.GetComponent<Player>() != null || currentCharacter.GetComponent<Companion>() != null)
        {
            Transition(BattleInputMode.Move);
        }
        else
        {
            Transition(BattleInputMode.Idle);
        }
    }

    public void SelectSkill(int i)
    {
        if (selectedSkill == i)
        {
            Transition(BattleInputMode.Move);
        }
        else
        {
            selectedSkill = i;
            if (selectedSkill != -1)
            {
                Transition(BattleInputMode.Skill);
            }
        }
    }

    public void UnSelectSkill()
    {
        selectedSkill = -1;
        Transition(BattleInputMode.Move);
    }

    public void RemoveCharacterFromBattle(Character character)
    {
        charactersOnBattle.Remove(character);
        
    }
}
