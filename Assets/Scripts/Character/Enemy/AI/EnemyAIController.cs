using UnityEngine;

public enum EnemyAIType
{
    Guardian,
    Assassin,
    Ranged,
    Caster
}

public class EnemyAIController : MonoBehaviour
{
    public Enemy character;
    private IEnemyPerception perception;
    private IEnemyDecision decision;

    public AIBlackboard blackboard;

    public void Start()
    {
        character = GetComponent<Enemy>();
        switch (character.aiType)
        {
            case EnemyAIType.Guardian:
                perception = new GuardianPerception();
                decision = new GuardianDecision();
                break;/*
            case EnemyAIType.Assassin:
                perception = new AssassinPerception();
                decision = new AssassinDecision();
                break;
            case EnemyAIType.Ranged:
                perception = new RangedPerception();
                break;
            case EnemyAIType.Caster:
                perception = new CasterPerception();
                break;*/
        }
    }

    public void TakeTurn()
    {
        var context = perception.UpdatePerception(character);

        var blackboard = decision.Decide(context);

        Execute(blackboard);
    }

    private void Execute(AIBlackboard blackboard)
    {
        //character.MoveTo(blackboard.moveDestination);
        //character.UseSkill(blackboard.selectedSkill, blackboard.chosenTarget);
    }
}
