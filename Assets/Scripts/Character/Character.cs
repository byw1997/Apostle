using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Collections;
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

public enum CharacterStatus
{
    Idle,
    Moving,
    Attacking,
    Casting,
    Dead
}
public class Character : MonoBehaviour
{
    [Header("Character Info")]
    public string characterName;
    [Header("Stat")]
    public int hp;
    public int currentHp;
    public int mp;
    public int currentMp;
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

    [Header("Skill")]
    public Skill[] skillSet = new Skill[6];
    public int[] skillLevel = new int[6];
    public List<int> acquiredSkills = new List<int>();

    [Header("Equipment")]
    public Helmet helmet;
    public Armor armor;
    public Glove glove;
    public Boots boots;
    public Weapon MainHandweapon;
    public Weapon SubHandWeapon;

    public CharacterStatus status;

    public void InitializeBattle()
    {
        currentHp = hp;
        currentMp = mp;
        currentActionPoint = actionPoint;
        status = CharacterStatus.Idle;
    }

    public void InitializeTurn()
    {
        currentActionPoint = actionPoint;
    }

    public Vector3 GridPositionToActualPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * 2.5f, 0, gridPos.y * 2.5f);
    }

    public void Move(Tile tile, Pathfinder.Node node)
    {
        StartCoroutine(MoveAlongPath(tile, node));
    }

    IEnumerator MoveAlongPath(Tile tile, Pathfinder.Node node)
    {
        status = CharacterStatus.Moving;
        List<Vector2Int> path = node.path;
        int totalCost = node.cost;
        Vector2Int currentPosition = tile.gridPos;
        Vector2Int nextPosition;
        currentActionPoint -= totalCost;
        for (int i = 1; i < path.Count; i++)
        {
            nextPosition = path[i];
            yield return StartCoroutine(MoveToPosition(nextPosition));
        }

        tileUnderCharacter.RemoveObjectOnTile();

        tile.Deploy(gameObject);
        
        status = CharacterStatus.Idle;
    }

    private IEnumerator MoveToPosition(Vector2Int targetPosition)//Move animation for each tile
    {
        float elapsedTime = 0f;
        float duration = 0.25f;
        Vector3 startingPosition = transform.position;
        Vector3 actualTargetPosition = GridPositionToActualPosition(targetPosition);
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, actualTargetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = actualTargetPosition;
    }
}
