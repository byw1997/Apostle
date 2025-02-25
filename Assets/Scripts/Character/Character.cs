using UnityEngine;

public enum CharacterType
{
    Player,
    Companion,
    Enemy
}

public class Character : MonoBehaviour
{
    [Header("Stat")]
    public int str { get; private set; }
    public int dex { get; private set; }
    public int con { get; private set; }
    public int kno { get; private set; }
    public int wis {  get; private set; }
    public int luk {  get; private set; }

    public Vector2Int gridPos;
}
