using UnityEngine;


public class Tile : MonoBehaviour
{
    [Header("Tile Data")]
    public TileData tileData;

    [Header("Tile Attribute")]
    public bool movable;
    public TileType type;
    public int moveCost;

    [SerializeField] private GameObject objectOnTile = null;
    public Vector2Int gridPos;

    private MeshRenderer mesh;
    private Color originalColor;
    private Color movableColor;
    private Color nonMovableColor;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        originalColor = mesh.material.color;
        movableColor = Color.blue;
        nonMovableColor = Color.red;
        TypeToCost();
    }

    private void Start()
    {
        Vector3 pos = transform.position;
        gridPos = new Vector2Int(Mathf.RoundToInt(pos.x / 2.5f), Mathf.RoundToInt(pos.z / 2.5f));
    }

    public string InformationToString()
    {
        string ret = string.Empty;
        ret += "Tile is on " + gridPos + "\n";
        ret += objectOnTile ? objectOnTile.name : "Nothing";
        ret += " on current Tile.";
        return ret;
    }

    public void OutputTileInfo()
    {
        Debug.Log(InformationToString());
    }

    public void HighlightMovable()
    {
        mesh.material.color = movableColor;
    }

    public void HighlightNonMovable()
    {
        mesh.material.color = nonMovableColor;
    }

    void TypeToCost()
    {
        switch (type)
        {
            case TileType.Flat:
                moveCost = 1;
                break;
            case TileType.Rough:
                moveCost = 2;
                break;
            case TileType.Harsh:
                moveCost = 3;
                break;
            case TileType.Impassable:
                moveCost = 65535;
                break;
        }  
    }
}
