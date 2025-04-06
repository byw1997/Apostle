using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Info")]
    public string enemyName;
    public int enemyId;

    [Header("Stat")]
    public int hp;
    public int mp;
    public int str;
    public int dex;
    public int con;
    public int kno;
    public int wis;
    public int luk;
    public int actionPoint;

    [Header("Range")]
    public int EngageRange;
    public MoveType moveType;
    public EngagementType engagementType;

    [Header("Class")]
    public ClassType mainClass;
    public ClassType subClass;

    [Header("Equipment")]
    public Helmet helmet;
    public Armor armor;
    public Glove glove;
    public Boots boots;
    public Weapon MainHandweapon;
    public Weapon SubHandWeapon;
}
