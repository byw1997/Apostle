using NUnit.Framework;
using UnityEngine;

public class Enemy : Character
{
    public int enemyID;

    private void Start()
    {
        type = CharacterType.Enemy;
    }

}
