using UnityEngine;

public enum WeaponType
{
    Dagger,
    Sword,
    GreatSword,
    Axe,
    GreatAxe,
    Hammer,
    GreatHammer,
    Flail,
    MorningStar,
    Rapier,
    Spear,
    Lance,
    Glaive,
    Bow,
    Crossbow,
    Staff,
    Wand,
    SmallShield,
    MediumShield,
    LargeShield,
    None
}

public enum WeaponHandedness
{
    OneHanded,
    TwoHanded
}

public enum StatBonusType
{
    Strength,
    Dexterity,
    Knowledge,
    Wisdom,
    Luck
}

public enum RangeType
{
    Orthogonal,
    Diagonal
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Equipment/Weapon")]
public class Weapon : Equipment
{
    public WeaponType weaponType;
    public int range;
    public RangeType rangeType;
    public int minimumDamage;
    public int maximumDamage;
    public float damageReduction;
    public StatBonusType statBonusType;
    public bool technical;
    public DamageType damageType;
    public int accuracy;
}
