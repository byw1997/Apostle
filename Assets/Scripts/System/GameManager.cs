using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [Header("Game States")]
    [SerializeField] private DefaultGameState defaultState;
    [SerializeField] private FieldGameState fieldState;
    [SerializeField] private BattleGameState battleState;
    [SerializeField] private DialogueGameState dialogueState;
    [SerializeField] private MenuGameState menuState;

    private GameStateContext gameStateContext;

    [SerializeField] private BattleManager battleManager;
    [SerializeField] private CharacterManager characterManager;

    public static GameManager Instance { get; private set; }

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

        gameStateContext = new GameStateContext(this);

        gameStateContext.Transition(battleState);
    }

    private void Update()
    {
        // 현재 상태에서의 로직을 처리
        gameStateContext.currentState.UpdateState();
    }

    // 상태 변경 함수
    public void ChangeState(GState state)
    {
        switch (state)
        {
            case GState.Default:
                gameStateContext.Transition(defaultState);
                break;
            case GState.Field:
                gameStateContext.Transition(fieldState);
                break;
            case GState.Battle:
                gameStateContext.Transition(battleState);
                break;
            case GState.Dialogue:
                gameStateContext.Transition(dialogueState);
                break;
            case GState.Menu:
                gameStateContext.Transition(menuState);
                break;
        }
        
    }

    
}
