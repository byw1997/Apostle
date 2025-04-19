using UnityEngine;

public interface IEnemyDecision
{
    AIBlackboard Decide(IEnemyContext context);
}
