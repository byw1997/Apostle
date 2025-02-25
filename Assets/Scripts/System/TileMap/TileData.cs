using UnityEngine;

public enum TileType
{
    Flat,
    Rough,
    Harsh,
    Impassable
}


[CreateAssetMenu(fileName = "TileData", menuName = "Scriptable Objects/TileData")]
public class TileData : ScriptableObject
{
    public TileType type;
}
