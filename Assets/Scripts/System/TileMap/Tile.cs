using UnityEngine;


public class Tile : MonoBehaviour
{
    [Header("Tile Data")]
    public TileData tileData;

    [Header("Tile Attribute")]
    public bool movable;
    public TileType type;
    public int moveCost;
    public bool deployable;

    public GameObject objectOnTile = null;
    public Vector2Int gridPos;

    private MeshRenderer mesh;
    private Color originalColor;
    private Color movableColor;
    private Color nonMovableColor;

    [SerializeField] GameObject highlightOverlay;
    [SerializeField] LineRenderer lineRenderer;

    MeshRenderer highlightMesh;

    [SerializeField] Material transparentMaterial;
    [SerializeField] Material movableMaterial;
    [SerializeField] Material nonMovableMaterial;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        TypeToCost();

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 5;
        lineRenderer.loop = true;
        lineRenderer.enabled = false;

        highlightMesh = highlightOverlay.GetComponent<MeshRenderer>();

        DrawOutline(true);
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

    public void Highlight(BattleInputMode bMode)
    {
        switch (bMode)
        {
            case BattleInputMode.Deploy:
                if (deployable && objectOnTile == null)
                {
                    HighlightMovable();
                }
                else
                {
                    HighlightNonMovable();
                }
                break;
            case BattleInputMode.Idle:
                break;
            case BattleInputMode.Skill:
                break;
            case BattleInputMode.Move:
                if (movable && objectOnTile == null)
                {
                    HighlightMovable();
                }
                else
                {
                    HighlightNonMovable();
                }
                break;
        }
    }

    public void HighlightMovable()
    {
        highlightMesh.material = movableMaterial;
    }

    public void HighlightNonMovable()
    {
        highlightMesh.material = nonMovableMaterial;
    }

    public void Unhighlight()
    {
        highlightMesh.material = transparentMaterial;
    }

    private void DrawOutline(bool enable)
    {
        if (enable)
        {
            Vector3[] corners = new Vector3[5];
            Bounds bounds = mesh.bounds;
            corners[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
            corners[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
            corners[2] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
            corners[3] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
            corners[4] = corners[0];

            lineRenderer.SetPositions(corners);
            lineRenderer.startColor = Color.black;
            lineRenderer.endColor = Color.black;
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
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

    public void Preview(GameObject character)
    {
        if (!character.activeSelf)
        {
            character.SetActive(true);
        }
        character.transform.position = transform.position;

    }

    public void RemoveObjectOnTile()
    {
        objectOnTile = null;
    }

    public void Deploy(GameObject character)
    {
        character.transform.position = transform.position;
        objectOnTile = character;
        Character characterComponent = character.GetComponent<Character>();
        characterComponent.gridPos = gridPos;
        characterComponent.tileUnderCharacter = this;
    }

}
