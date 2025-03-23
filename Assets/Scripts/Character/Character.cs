using UnityEngine;

public enum CharacterType
{
    Player,
    Companion,
    Enemy
}

public enum MoveType
{
    Orthogonal,
    Diagonal
}

public enum EngagementType
{
    Orthogonal,
    Diagonal
}

public enum ClassType
{
    Fighter,
    Wizard,
    Rogue,
    Priest,
    None
}

public class Character : MonoBehaviour
{
    [Header("Character Info")]
    public string characterName;
    [Header("Stat")]
    public int str;
    public int dex;
    public int con;
    public int kno;
    public int wis;
    public int luk;
    public int actionPoint;

    public Tile tileUnderCharacter;
    public Vector2Int gridPos;

    public CharacterType type;

    [Header("Range")]
    public int currentActionPoint;
    public int EngageRange;
    public MoveType moveType;
    public EngagementType engagementType;

    [Header("Class")]
    public ClassType mainClass;
    public ClassType subClass;

    public void InitializeTurn()
    {
        currentActionPoint = actionPoint;
    }

    public void Move(Tile tile, Pathfinder.Node node)
    {
        tileUnderCharacter.RemoveObjectOnTile();
        currentActionPoint -= node.cost;
        tile.Deploy(gameObject);
    }
}
