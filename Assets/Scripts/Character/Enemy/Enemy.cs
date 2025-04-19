using UnityEngine;
#if UNITY_EDITOR
using NUnit.Framework;
#endif

public class Enemy : Character
{
    public int enemyID;

    public EnemyAIType aiType;

    private void Start()
    {
        type = CharacterType.Enemy;
    }

}
