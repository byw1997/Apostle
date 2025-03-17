using System.Collections.Generic;
using UnityEngine;

public class TilemapManager : MonoBehaviour
{
    private Dictionary<Vector2Int, Tile> tileMap = new Dictionary<Vector2Int, Tile>();

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

    public void HighlightAll(BattleInputMode bMode)
    {
        foreach(Tile tile in tileMap.Values)
        {
            tile.Highlight(bMode);
        }
    }

}
