using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
#if UNITY_EDITOR
using NUnit.Framework;
#endif

public class Pathfinder
{
    public class Node
    {
        public Vector2Int position;
        public int cost;
        public List<Vector2Int> path;

        public Node(Vector2Int pos, int c, List<Vector2Int> p)
        {
            position = pos;
            cost = c;
            path = new List<Vector2Int>(p);
        }

    }

    Dictionary<Vector2Int, Tile> tileMap;

    Vector2Int[] orthogonalDirections = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    Vector2Int[] diagonalDirections = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.up + Vector2Int.left, Vector2Int.up + Vector2Int.right, Vector2Int.down + Vector2Int.left, Vector2Int.down + Vector2Int.right };

    public Pathfinder(Dictionary<Vector2Int, Tile> tileMap)
    {
        this.tileMap = tileMap;
    }

    public Dictionary<Vector2Int, Node> CalculateMoveRange(Character character, MoveType moveType)
    {
        Vector2Int startPos = character.gridPos;
        int maxCost = character.currentActionPoint;
        Dictionary<Vector2Int, Node> reachableTiles = new Dictionary<Vector2Int, Node>();
        Queue<Node> queue = new Queue<Node>();

        queue.Enqueue(new Node(startPos, 0, new List<Vector2Int> { startPos }));
        reachableTiles[startPos] = new Node(startPos, 0, new List<Vector2Int> { startPos });

         Vector2Int[] directions = null;

        switch (moveType)
        {
            case MoveType.Orthogonal:
                directions = orthogonalDirections;
                break;
            case MoveType.Diagonal:
                directions = diagonalDirections;
                break;
        }

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            foreach (Vector2Int dir in directions)
            {
                Vector2Int nextPos = current.position + dir;
                if (!tileMap.ContainsKey(nextPos))
                {
                    continue;
                }
                int newCost = current.cost + tileMap[nextPos].moveCost;

                if (newCost > maxCost)
                    continue;

                Tile nextTile = tileMap[nextPos];

                if (!nextTile.movable || nextTile.objectOnTile)
                    continue;

                if (!reachableTiles.ContainsKey(nextPos) || newCost < reachableTiles[nextPos].cost)
                {
                    List<Vector2Int> newPath = new List<Vector2Int>(current.path) { nextPos };
                    Node newNode = new Node(nextPos, newCost, newPath);

                    reachableTiles[nextPos] = newNode;
                    queue.Enqueue(newNode);
                }
            }
        }

        reachableTiles.Remove(startPos);

        return reachableTiles;
    }

    public Dictionary<Vector2Int, Node> CalculateSkillRange(Character character, int index)
    {
        if(index < 0)
        {
            Debug.LogError("Invalid skill index");
            return null;
        }
        Vector2Int startPos = character.gridPos;
        Skill skill = character.skillSet[index];
        int level = character.skillLevel[index];
        SkillRangeType rangeType = skill.skillRangeType;

        
        Dictionary<Vector2Int, Node> reachableTiles = new Dictionary<Vector2Int, Node>();
        Queue<Node> queue = new Queue<Node>();

        queue.Enqueue(new Node(startPos, 0, new List<Vector2Int> { startPos }));
        reachableTiles[startPos] = new Node(startPos, 0, new List<Vector2Int> { startPos });

        Vector2Int[] directions = null;
        int range = 0;
        switch (rangeType)
        {
            case SkillRangeType.Orthogonal:
                range = skill.skillRanges[level];
                directions = orthogonalDirections;
                break;
            case SkillRangeType.Diagonal:
                range = skill.skillRanges[level];
                directions = diagonalDirections;
                break;
            case SkillRangeType.Circle:
                range = skill.skillRanges[level];
                return CalculateCircleRange(startPos, range);
            case SkillRangeType.Weapon:
                Weapon weapon = skill.usedBySubWeapon? character.subHandWeapon : character.mainHandWeapon;
                range = weapon.range;
                switch (weapon.rangeType)
                {
                    case RangeType.Orthogonal:
                        directions = orthogonalDirections;
                        break;
                    case RangeType.Diagonal:
                        directions = diagonalDirections;
                        break;
                }
                break;
        }
        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            foreach (Vector2Int dir in directions)
            {
                Vector2Int nextPos = current.position + dir;
                if (!tileMap.ContainsKey(nextPos))
                {
                    continue;
                }
                int newCost = current.cost + 1;

                if (newCost > range)
                    continue;

                Tile nextTile = tileMap[nextPos];

                if (!reachableTiles.ContainsKey(nextPos) || newCost < reachableTiles[nextPos].cost)
                {
                    List<Vector2Int> newPath = new List<Vector2Int>(current.path) { nextPos };
                    Node newNode = new Node(nextPos, newCost, newPath);

                    reachableTiles[nextPos] = newNode;
                    queue.Enqueue(newNode);
                }
            }
        }

        return reachableTiles;
    }

    public Dictionary<Vector2Int, Node> CalculateSkillArea(Tile tile, AOESkill skill, int level)
    {
        Vector2Int startPos = tile.gridPos;
        SkillAreaType areaType = skill.skillAreaType;
        Dictionary<Vector2Int, Node> reachableTiles = new Dictionary<Vector2Int, Node>();

        Vector2Int[] directions = null;
        switch (areaType)
        {
            case SkillAreaType.Orthogonal:
                directions = orthogonalDirections;
                break;
            case SkillAreaType.Diagonal:
                directions = diagonalDirections;
                break;
            case SkillAreaType.Circle:
                return CalculateCircleRange(startPos, skill.skillAreaRanges[level]);
            case SkillAreaType.Custom:
                CustomAOESkill customAOESkill = skill as CustomAOESkill;
                foreach (Vector2Int pos in customAOESkill.GetCustomArea(startPos, level))
                {
                    if (tileMap.ContainsKey(pos))
                    {
                        reachableTiles[pos] = new Node(pos, 0, new List<Vector2Int> { pos });
                    }
                }
                return reachableTiles;
        }
        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(new Node(startPos, 0, new List<Vector2Int> { startPos }));
        int range = skill.skillAreaRanges[level];
        reachableTiles[startPos] = new Node(startPos, 0, new List<Vector2Int> { startPos });
        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();
            foreach (Vector2Int dir in directions)
            {
                Vector2Int nextPos = current.position + dir;
                if (!tileMap.ContainsKey(nextPos))
                {
                    continue;
                }
                int newCost = current.cost + 1;
                if (newCost > range)
                    continue;
                Tile nextTile = tileMap[nextPos];
                if (!reachableTiles.ContainsKey(nextPos) || newCost < reachableTiles[nextPos].cost)
                {
                    List<Vector2Int> newPath = new List<Vector2Int>(current.path) { nextPos };
                    Node newNode = new Node(nextPos, newCost, newPath);
                    reachableTiles[nextPos] = newNode;
                    queue.Enqueue(newNode);
                }
            }
        }
        return reachableTiles;
    }

    public Dictionary<Vector2Int, Node> CalculateCircleRange(Vector2Int startPos, int range)
    {
        Dictionary<Vector2Int, Node> reachableTiles = new Dictionary<Vector2Int, Node>();
        
        int minX = startPos.x - range;
        int maxX = startPos.x + range;
        int minY = startPos.y - range;
        int maxY = startPos.y + range;

        for(int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (tileMap.ContainsKey(pos))
                {
                    int distance = Mathf.FloorToInt(Vector2Int.Distance(startPos, pos));
                    if (distance <= range)
                    {
                        reachableTiles[pos] = new Node(pos, distance, new List<Vector2Int> { pos });
                    }
                }
            }
        }

        return reachableTiles;
    }

}
