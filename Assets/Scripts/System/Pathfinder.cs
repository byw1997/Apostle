using System.Collections.Generic;
using UnityEngine;

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

    public static Dictionary<Vector2Int, Node> CalculateMoveRangeByOrthogonal(Vector2Int startPos, int maxCost, Dictionary<Vector2Int, Tile> tileMap)
    {
        Dictionary<Vector2Int, Node> reachableTiles = new Dictionary<Vector2Int, Node>();
        Queue<Node> queue = new Queue<Node>();

        // ���� ��带 ť�� �߰�
        queue.Enqueue(new Node(startPos, 0, new List<Vector2Int> { startPos }));
        reachableTiles[startPos] = new Node(startPos, 0, new List<Vector2Int> { startPos });

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            foreach (Vector2Int dir in directions)
            {
                Vector2Int nextPos = current.position + dir;
                int newCost = current.cost + tileMap[nextPos].moveCost;

                // Ÿ�ϸʿ� �������� �ʰų�, ���ο� ����� maxCost �ʰ��ϸ� �ǳʶڴ�.
                if (!tileMap.ContainsKey(nextPos) || newCost > maxCost)
                    continue;

                Tile nextTile = tileMap[nextPos];

                // �ش� Ÿ���� �̵� �Ұ����� ��� ��ŵ (��: ��, ��ֹ�)
                if (!nextTile.movable)
                    continue;

                // �� ���� ������� �湮�� �� �ִٸ� ����
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

    public static Dictionary<Vector2Int, Node> CalculateMoveRangeByDiagonal(Vector2Int startPos, int maxCost, Dictionary<Vector2Int, Tile> tileMap)
    {
        Dictionary<Vector2Int, Node> reachableTiles = new Dictionary<Vector2Int, Node>();
        Queue<Node> queue = new Queue<Node>();

        // ���� ��带 ť�� �߰�
        queue.Enqueue(new Node(startPos, 0, new List<Vector2Int> { startPos }));
        reachableTiles[startPos] = new Node(startPos, 0, new List<Vector2Int> { startPos });

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, 
            Vector2Int.up + Vector2Int.left, Vector2Int.up + Vector2Int.right,  Vector2Int.down + Vector2Int.left, Vector2Int.down + Vector2Int.right};

        while (queue.Count > 0)
        {
            Node current = queue.Dequeue();

            foreach (Vector2Int dir in directions)
            {
                Vector2Int nextPos = current.position + dir;
                int newCost = current.cost + tileMap[nextPos].moveCost;

                // Ÿ�ϸʿ� �������� �ʰų�, ���ο� ����� maxCost �ʰ��ϸ� �ǳʶڴ�.
                if (!tileMap.ContainsKey(nextPos) || newCost > maxCost)
                    continue;

                Tile nextTile = tileMap[nextPos];

                // �ش� Ÿ���� �̵� �Ұ����� ��� ��ŵ (��: ��, ��ֹ�)
                if (!nextTile.movable)
                    continue;

                // �� ���� ������� �湮�� �� �ִٸ� ����
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
}
