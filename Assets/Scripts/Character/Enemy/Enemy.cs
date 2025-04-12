using UnityEngine;
#if UNITY_EDITOR
using NUnit.Framework;
#endif

public class Enemy : Character
{
    public int enemyID;

    private void Start()
    {
        type = CharacterType.Enemy;
    }

}
