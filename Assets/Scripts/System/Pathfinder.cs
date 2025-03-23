using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

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

        // 시작 노드를 큐에 추가
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

        Assert.IsNotNull(directions);

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

                // 타일맵에 존재하지 않거나, 새로운 비용이 maxCost 초과하면 건너뛴다.
                if (newCost > maxCost)
                    continue;

                Tile nextTile = tileMap[nextPos];

                // 해당 타일이 이동 불가능한 경우 스킵 (예: 벽, 장애물)
                if (!nextTile.movable)
                    continue;

                // 더 적은 비용으로 방문할 수 있다면 갱신
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

}
