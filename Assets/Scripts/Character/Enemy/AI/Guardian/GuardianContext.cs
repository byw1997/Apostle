using System.Collections.Generic;
using UnityEngine;

public enum GuardianState
{
    Frontline,
    Protecting
}
public class GuardianContext : IEnemyContext
{
    public Character self;
    public Vector2Int selfPos;

    public List<Character> playerCharacters;
    public List<Character> enemies;

    public Vector2 playerFormationCenter;
    public Vector2 enemyFormationCenter;

    public Character vulnerableEnemy;
    public List<Character> threats;
    public List<Vector2Int> walkableTiles;
    public GuardianState state;
}
