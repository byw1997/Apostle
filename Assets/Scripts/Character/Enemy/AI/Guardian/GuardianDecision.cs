using UnityEngine;

public class GuardianDecision : IEnemyDecision
{
    public AIBlackboard Decide(IEnemyContext baseContext)
    {
        AIBlackboard blackboard = new AIBlackboard();

        return blackboard;
    }
}
