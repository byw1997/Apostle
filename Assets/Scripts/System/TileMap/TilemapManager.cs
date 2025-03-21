using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TilemapManager : MonoBehaviour
{
    public Dictionary<Vector2Int, Tile> tileMap = new Dictionary<Vector2Int, Tile>();

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
