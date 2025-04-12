using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

public class TilemapManager : MonoBehaviour
{
    public static Dictionary<Vector2Int, Tile> tileMap = new Dictionary<Vector2Int, Tile>();

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        tileMap.Clear();
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Plane");

        foreach (GameObject tile in tiles)
        {
            Tile tileData = tile.GetComponent<Tile>();
            tileMap[tileData.gridPos] = tileData;
        }
    }

    public void HighlightAll(BattleInputMode bMode, Dictionary<Vector2Int, Pathfinder.Node> reachableTiles = null)
    {
        switch (bMode)
        {
            case BattleInputMode.Deploy:
                foreach (Tile tile in tileMap.Values)
                {
                    tile.Highlight(bMode);
                }
                break;
            case BattleInputMode.Move:
                foreach (Tile tile in tileMap.Values)
                {
                    if (reachableTiles.ContainsKey(tile.gridPos))
                    {
                        tile.Highlight(bMode);
                    }
                }
                break;
            case BattleInputMode.Skill:
                foreach (Tile tile in tileMap.Values)
                {
                    if (reachableTiles.ContainsKey(tile.gridPos))
                    {
                        tile.Highlight(bMode);
                    }
                }
                break;
        }
    }

    public void HighlightTile(Vector2Int gridPos, BattleInputMode bMode, bool isOnEffect = false)
    {
        if (tileMap.ContainsKey(gridPos))
        {
            tileMap[gridPos].Highlight(bMode, isOnEffect);
        }
    }

    public void UnhighlightAll()
    {
        foreach (Tile tile in tileMap.Values)
        {
            tile.Unhighlight();
        }
    }

}
